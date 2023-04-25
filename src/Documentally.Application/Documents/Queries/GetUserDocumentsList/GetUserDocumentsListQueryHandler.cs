// <copyright file="GetUserDocumentsListQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Documents.Results;
using FluentResults;

namespace Documentally.Application.UserDocuments.Queries.GetUserDocumentssList;

/// <summary>
/// Implementation for the Get List of Documents for the User Query.
/// </summary>
public class GetUserDocumentsListQueryHandler : IQueryHandler<GetUserDocumentsListQuery, List<DocumentResult>>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserDocumentsListQueryHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected IDocumentRepository.</param>
    public GetUserDocumentsListQueryHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<List<DocumentResult>>> Handle(GetUserDocumentsListQuery query, CancellationToken cancellationToken)
    {
        var result = new List<DocumentResult>();

        var documentsResult = await documentRepository.GetAllDocumentsFromUserIdAsync(query.UserId);
        if (documentsResult.IsSuccess)
        {
            foreach (var documentResult in documentsResult.Value)
            {
                result.Add(new DocumentResult(
                    documentResult.Id.Value,
                    documentResult.OwnerId.Value,
                    documentResult.Name,
                    documentResult.Description,
                    documentResult.Category,
                    documentResult.Size,
                    documentResult.BlobUrl,
                    documentResult.CloudFileName,
                    documentResult.PostedAtUtc));
            }
        }

        return Result.Ok(result);
    }
}