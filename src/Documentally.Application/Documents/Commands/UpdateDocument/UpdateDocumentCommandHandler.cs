// <copyright file="UpdateDocumentCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Documents.Results;
using Documentally.Domain.Document;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.UpdateDocument;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class UpdateDocumentCommandHandler : ICommandHandler<UpdateDocumentCommand, DocumentResult>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected UserRepository.</param>
    public UpdateDocumentCommandHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<DocumentResult>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentResult = Document.Create(
            request.Id,
            new UserId(request.UserId),
            request.FileName,
            request.Description,
            request.Category,
            request.Size,
            request.BlobUrl,
            request.CloudFileName);

        if (documentResult.IsSuccess)
        {
            var updateResult = await documentRepository.UpdateAsync(documentResult.Value);
            if (updateResult.IsSuccess)
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
                    documentResult.Value.PostedAtUtc));
            }
            else
            {
                return Result.Fail(updateResult.Errors);
            }
        }
        else
        {
            return Result.Fail(documentResult.Errors);
        }
    }
}
