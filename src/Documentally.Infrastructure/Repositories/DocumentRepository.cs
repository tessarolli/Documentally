// <copyright file="DocumentRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Common.Errors;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Document;
using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The Document Repository.
/// </summary>
public class DocumentRepository : IDocumentRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    public DocumentRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<DocumentRepository> logger)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<Document>> GetByIdAsync(DocId documentId)
    {
        logger.LogInformation("DocumentRepository.GetByIdAsync({id})", documentId.Value);

        var sql = "SELECT * FROM documents WHERE id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var documentDto = await connection.QueryFirstOrDefaultAsync<DocumentDto>(sql, parameters);

            await connection.CloseAsync();

            return CreateDocumentResultFromdocumentDto(documentDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetByIdsAsync(DocId[] ids)
    {
        logger.LogInformation("DocumentRepository.GetByIdsAsync({ids})", ids);

        var sql = "SELECT * FROM documents WHERE id = ANY(@Ids)";

        var parameters = new
        {
            Ids = ids.Select(x => x.Value),
        };

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var documentDtos = await connection.QueryAsync<DocumentDto>(sql, parameters);

            await connection.CloseAsync();

            var documents = documentDtos
                .Select(CreateDocumentResultFromdocumentDto)
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList();

            return documents;
        }
        catch (Exception ex)
        {
            ex.GetPrettyMessage(logger);
        }

        return new List<Document>();
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetAllDocumentsFromUserIdAsync(long userId)
    {
        logger.LogInformation("DocumentRepository.GetAllAsync()");

        var sql = "SELECT * FROM documents WHERE owner_id = @Id";

        var parameters = new
        {
            Id = userId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var documentDtos = await connection.QueryAsync<DocumentDto>(sql, parameters);

            await connection.CloseAsync();

            var documents = documentDtos
                .Select(CreateDocumentResultFromdocumentDto)
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList();

            return documents;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetAllDocumentsSharedToUserIdAsync(long userId)
    {
        logger.LogInformation("DocumentRepository.GetAllDocumentsSharedToUserIdAsync()");

        var sql = @"
            SELECT DISTINCT
                d.*
            FROM 
                documents d 
            JOIN 
                document_access da
            ON 
                da.document_id = d.id
            WHERE 
                da.user_id = @UserId 
                OR 
                da.group_id IN (
                    SELECT 
                        group_id 
                    FROM 
                        group_members 
                    WHERE 
                        user_id = @UserID
                )";

        var parameters = new
        {
            UserId = userId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var documentDtos = await connection.QueryAsync<DocumentDto>(sql, parameters);

            await connection.CloseAsync();

            var documents = documentDtos
                .Select(CreateDocumentResultFromdocumentDto)
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList();

            return documents;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> VerifyUserIdHasAccessToDocIdAsync(DocId documentId, long userId)
    {
        logger.LogInformation("DocumentRepository.VerifyUserIdHasAccessToDocIdAsync()");

        var sql = @"
            SELECT 
                COUNT(*) > 0 as has_access
            FROM 
                document_access
            WHERE 
                document_id = @DocumentId
                AND 
                (  
                    user_id = @UserId 
                    OR    
                    group_id IN (
                        SELECT 
                            id       
                        FROM 
                            groups        
                        WHERE 
                            user_id = @UserId)
                );";

        var parameters = new
        {
            DocumentId = documentId.Value,
            UserId = userId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var has_access = await connection.QuerySingleAsync<bool>(sql, parameters);

            await connection.CloseAsync();

            if (has_access)
            {
                return Result.Ok();
            }

            return Result.Fail(new UnauthorizedError());
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<Document>> AddAsync(Document document)
    {
        logger.LogInformation("DocumentRepository.AddAsync()");

        var sql = @"
            INSERT INTO 
                documents ( 
                    owner_id,
                    doc_name,
                    doc_description,
                    doc_category,
                    doc_size,
                    blob_url,
                    cloud_file_name
                )
            VALUES (
                @OwnerId,
                @Name,
                @Description,
                @Category,
                @Size,  
                @BlobUrl,
                @CloudFileName
            ) 
            RETURNING 
                id
            ;";

        var parameters = new
        {
            OwnerId = document.OwnerId.Value,
            document.Name,
            document.Description,
            document.Category,
            document.Size,
            document.BlobUrl,
            document.CloudFileName,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var newId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            await connection.CloseAsync();

            return await GetByIdAsync(new DocId(newId));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<Document>> UpdateAsync(Document document)
    {
        logger.LogInformation("DocumentRepository.UpdateAsync({Document})", document);

        var sql = @"
            UPDATE 
                documents 
            SET
                owner_id = @OwnerId,
                doc_name = @Name,
                doc_description = @Description,
                doc_category = @Category,
                doc_size = @Size,
                blob_url = @BlobUrl,
                cloud_file_name = @CloudFileName
            WHERE 
                id = @Id";

        var parameters = new
        {
            Id = document.Id.Value,
            OwnerId = document.OwnerId.Value,
            document.Name,
            document.Description,
            document.Category,
            document.Size,
            document.BlobUrl,
            document.CloudFileName,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount > 0)
            {
                return Result.Ok(document);
            }

            return Result.Fail(new NotFoundError());
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(DocId documentId)
    {
        logger.LogInformation("DocumentRepository.RemoveAsync({DocumentId})", documentId.Value);

        await RemoveDocumentFromAllSharedAccessAsync(documentId);

        var sql = "DELETE FROM documents WHERE id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount == 0)
            {
                return Result.Fail(new NotFoundError());
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> ShareDocumentWithGroupAsync(DocId documentId, long groupId)
    {
        logger.LogInformation("DocumentRepository.ShareDocumentWithGroupAsync({a},{b})", documentId.Value, groupId);

        var sql = @"
            INSERT INTO 
                document_access 
                (
                    document_id, 
                    group_id
                ) 
            VALUES 
            (
                @DocumentId, 
                @GroupId
            )";

        var parameters = new
        {
            DocumentId = documentId.Value,
            GroupId = groupId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount > 0)
            {
                return Result.Ok();
            }

            return Result.Fail("Couldnt share this document with the group, because some internal error ocurred in the server.");
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> ShareDocumentWithUserAsync(DocId documentId, long userId)
    {
        logger.LogInformation("DocumentRepository.ShareDocumentWithUserAsync({a},{b})", documentId.Value, userId);

        var sql = @"
            INSERT INTO 
                document_access 
                (
                    document_id, 
                    user_id
                ) 
            VALUES 
            (
                @DocumentId, 
                @UserId
            )";

        var parameters = new
        {
            DocumentId = documentId.Value,
            UserId = userId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount > 0)
            {
                return Result.Ok();
            }

            return Result.Fail("Couldnt share this document with the user, because some internal error ocurred in the server.");
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveDocumentShareFromUserAsync(DocId documentId, long userId)
    {
        logger.LogInformation("DocumentRepository.RemoveDocumentShareFromUserAsync({DocumentId},{userId})", documentId.Value, userId);

        var sql = "DELETE FROM document_access WHERE document_id = @Id AND user_id = @GroupId";

        var parameters = new
        {
            Id = documentId.Value,
            UserId = userId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount > 0)
            {
                return Result.Ok();
            }

            return Result.Fail(new NotFoundError());
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveDocumentShareFromGroupAsync(DocId documentId, long groupId)
    {
        logger.LogInformation("DocumentRepository.RemoveDocumentShareFromGroupAsync({DocumentId},{groupId})", documentId.Value, groupId);

        var sql = "DELETE FROM document_access WHERE document_id = @Id AND group_id = @GroupId";

        var parameters = new
        {
            Id = documentId.Value,
            GroupId = groupId,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            if (affectedRecordsCount > 0)
            {
                return Result.Ok();
            }

            return Result.Fail(new NotFoundError());
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <summary>
    /// Creates a new instance of the Document Entity, using the Create factory method.
    /// </summary>
    /// <param name="documentDto">The Document Data Transfer Object.</param>
    /// <returns>An Result indicating the status of the operation.</returns>
    private static Result<Document> CreateDocumentResultFromdocumentDto(DocumentDto? documentDto)
    {
        if (documentDto is not null)
        {
            var documentResult = Document.Create(
                documentDto.id,
                new UserId(documentDto.owner_id),
                documentDto.doc_name,
                documentDto.doc_description,
                documentDto.doc_category,
                documentDto.doc_size,
                documentDto.blob_url,
                documentDto.cloud_file_name,
                documentDto.posted_date_utc,
                new Lazy<User>(() => User.Empty));

            if (documentResult.IsSuccess)
            {
                return Result.Ok(documentResult.Value);
            }

            return Result.Fail(documentResult.Errors);
        }

        return Result.Fail(new NotFoundError($"Document with the provided E-Mail was not found."));
    }

    /// <summary>
    /// Removes this document from all shared instances.
    /// </summary>
    /// <param name="documentId">The document Id to remove.</param>
    /// <returns>awaitable task.</returns>
    private async Task RemoveDocumentFromAllSharedAccessAsync(DocId documentId)
    {
        logger.LogInformation("DocumentRepository.RemoveAsync({DocumentId})", documentId.Value);

        var sql = "DELETE FROM document_access WHERE document_id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var affectedRecordsCount = await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();
        }
        catch (Exception ex)
        {
            ex.GetPrettyMessage(logger);
        }
    }
}
