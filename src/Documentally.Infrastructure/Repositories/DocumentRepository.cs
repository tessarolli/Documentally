// <copyright file="DocumentRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Common.Errors;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Document;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Utilities;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The Document Repository.
/// </summary>
public class DocumentRepository : IDocumentRepository
{
    private readonly ILogger logger;
    private readonly DapperUtility db;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    public DocumentRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<DocumentRepository> logger)
    {
        this.logger = logger;
        this.db = new DapperUtility(postgresSqlConnectionFactory);
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

        var documentDto = await db.QueryFirstOrDefaultAsync<DocumentDto>(sql, parameters);

        return await CreateDocumentResultFromDocumentDtoAsync(documentDto);
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetByIdsAsync(DocId[] ids)
    {
        var sql = "SELECT * FROM documents WHERE id = ANY(@Ids)";

        var parameters = new
        {
            Ids = ids.Select(x => x.Value),
        };

        var documentDtos = await db.QueryAsync<DocumentDto>(sql, parameters);

        var documents = documentDtos
            .Select(async x => await CreateDocumentResultFromDocumentDtoAsync(x))
            .Where(x => x.Result.IsSuccess)
            .Select(x => x.Result.Value)
            .ToList();

        return documents;
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetAllDocumentsFromUserIdAsync(long userId)
    {
        var sql = "SELECT * FROM documents WHERE owner_id = @Id";

        var parameters = new
        {
            Id = userId,
        };

        var documentDtos = await db.QueryAsync<DocumentDto>(sql, parameters);

        var documents = documentDtos
            .Select(async x => await CreateDocumentResultFromDocumentDtoAsync(x))
             .Where(x => x.Result.IsSuccess)
            .Select(x => x.Result.Value)
            .ToList();

        return documents;
    }

    /// <inheritdoc/>
    public async Task<Result<List<Document>>> GetAllDocumentsSharedToUserIdAsync(long userId)
    {
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

        var documentDtos = await db.QueryAsync<DocumentDto>(sql, parameters);

        var documents = documentDtos
            .Select(async x => await CreateDocumentResultFromDocumentDtoAsync(x))
            .Where(x => x.Result.IsSuccess)
            .Select(x => x.Result.Value)
            .ToList();

        return documents;
    }

    /// <inheritdoc/>
    public async Task<Result> VerifyUserIdHasAccessToDocIdAsync(DocId documentId, long userId)
    {
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

        var has_access = await db.QueryFirstOrDefaultAsync<bool>(sql, parameters);

        if (has_access)
        {
            return Result.Ok();
        }

        return Result.Fail(new UnauthorizedError());
    }

    /// <inheritdoc/>
    public async Task<Result<Document>> AddAsync(Document document)
    {
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

        var newId = await db.ExecuteScalarAsync(sql, parameters);

        return await GetByIdAsync(new DocId(newId));
    }

    /// <inheritdoc/>
    public async Task<Result<Document>> UpdateAsync(Document document)
    {
        var updateDocumentSql = @"
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

        var updateDocumentParameters = new
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

        await db.ExecuteAsync(updateDocumentSql, updateDocumentParameters);

        await RemoveDocumentFromAllSharedAccessAsync(document.Id);

        var insertSharedAccessSql = @"
            INSERT INTO  
                document_access 
                ( 
                    document_id, 
                    user_id, 
                    group_id
                )
            VALUES 
            (
                @DocumentId,
                @UserId,
                @GroupId
            )";

        /*                      docId, GroupId, UserId */
        var paramsList = new List<(long, long?, long?)>();
        paramsList.AddRange(document.SharedGroupIds.Select(x => (document.Id.Value, (long?)x.Value, (long?)null)));
        paramsList.AddRange(document.SharedUserIds.Select(x => (document.Id.Value, (long?)null, (long?)x.Value)));

        // Create an anonymous object for holding the parameters based on ParamsList Tuple.
        var insertParamsEnumerable = paramsList.Select(x => new { DocumentId = x.Item1, GroupId = x.Item2, UserId = x.Item3 });

        await db.ExecuteAsync(insertSharedAccessSql, insertParamsEnumerable);

        return Result.Ok(document);
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(DocId documentId)
    {
        await RemoveDocumentFromAllSharedAccessAsync(documentId);

        var sql = "DELETE FROM documents WHERE id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        await db.ExecuteAsync(sql, parameters);

        return Result.Ok();
    }

    /// <summary>
    /// Creates a new instance of the Document Entity, using the Create factory method.
    /// </summary>
    /// <param name="documentDto">The Document Data Transfer Object.</param>
    /// <returns>An Result indicating the status of the operation.</returns>
    private async Task<Result<Document>> CreateDocumentResultFromDocumentDtoAsync(DocumentDto? documentDto)
    {
        if (documentDto is not null)
        {
            var documentAccess = await GetListOfDocumentAccessForDocIdAsync(new DocId(documentDto.id));

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
                new Lazy<User>(() => User.Empty),
                documentAccess?.Where(x => x.group_id is not null).Select(x => new GroupId(x.group_id)).ToList(),
                documentAccess?.Where(x => x.user_id is not null).Select(x => new UserId(x.user_id)).ToList());

            if (documentResult.IsSuccess)
            {
                return Result.Ok(documentResult.Value);
            }

            return Result.Fail(documentResult.Errors);
        }

        return Result.Fail(new NotFoundError($"Document with the provided E-Mail was not found."));
    }

    private async Task<IEnumerable<DocumentAccessDto>> GetListOfDocumentAccessForDocIdAsync(DocId documentId)
    {
        var sql = "SELECT * FROM document_access WHERE document_id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        return await db.QueryAsync<DocumentAccessDto>(sql, parameters);
    }

    private async Task RemoveDocumentFromAllSharedAccessAsync(DocId documentId)
    {
        var sql = "DELETE FROM document_access WHERE document_id = @Id";

        var parameters = new
        {
            Id = documentId.Value,
        };

        await db.ExecuteAsync(sql, parameters);
    }
}
