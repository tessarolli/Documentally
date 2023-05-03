// <copyright file="GetDocumentsSharedWithUserQueryHandler.cs" company="Documentally">
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
public class GetDocumentsSharedWithUserQueryHandler : IQueryHandler<GetDocumentsSharedWithUserQuery, List<DocumentResult>>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDocumentsSharedWithUserQueryHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected IDocumentRepository.</param>
    public GetDocumentsSharedWithUserQueryHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<List<DocumentResult>>> Handle(GetDocumentsSharedWithUserQuery query, CancellationToken cancellationToken)
    {
        var result = new List<DocumentResult>();

        var documentsResult = await documentRepository.GetAllDocumentsSharedToUserIdAsync(query.UserId);
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
                    documentResult.SharedGroupIds.Select(x => x.Value).ToList(),
                    documentResult.SharedUserIds.Select(x => x.Value).ToList(),
                    documentResult.PostedAtUtc));
            }
        }

        return Result.Ok(result);
    }
}