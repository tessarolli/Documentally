// <copyright file="GetDocumentByIdQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Documents.Results;
using Documentally.Domain.Doc.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Queries.GetDocumentById;

/// <summary>
/// Implementation for the GetDocumentByIdQuery.
/// </summary>
public class GetDocumentByIdQueryHandler : IQueryHandler<GetDocumentByIdQuery, DocumentResult>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDocumentByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected DocumentRepository.</param>
    public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<DocumentResult>> Handle(GetDocumentByIdQuery query, CancellationToken cancellationToken)
    {
        var documentResult = await documentRepository.GetByIdAsync(new DocId(query.Id));
        if (documentResult.IsSuccess)
        {
            return Result.Ok(new DocumentResult(
                documentResult.Value.Id.Value,
                documentResult.Value.OwnerId.Value,
                documentResult.Value.Name,
                documentResult.Value.Description,
                documentResult.Value.Category,
                documentResult.Value.Size,
                documentResult.Value.BlobUrl,
                documentResult.Value.CloudFileName,
                documentResult.Value.SharedGroupIds.Select(x => x.Value).ToList(),
                documentResult.Value.SharedUserIds.Select(x => x.Value).ToList(),
                documentResult.Value.PostedAtUtc));
        }

        return Result.Fail(documentResult.Errors);
    }
}