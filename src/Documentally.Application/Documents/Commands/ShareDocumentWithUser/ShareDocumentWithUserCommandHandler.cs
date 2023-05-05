// <copyright file="ShareDocumentWithUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithUser;

/// <summary>
/// Implementation for the ShareDocumentWithUserCommand.
/// </summary>
public class ShareDocumentWithUserCommandHandler : ICommandHandler<ShareDocumentWithUserCommand>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareDocumentWithUserCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected UserRepository.</param>
    public ShareDocumentWithUserCommandHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(ShareDocumentWithUserCommand request, CancellationToken cancellationToken)
    {
        // Gets the Document by its Id.
        var documentResult = await documentRepository.GetByIdAsync(new DocId(request.DocumentId));
        if (documentResult.IsFailed)
        {
            return Result.Fail(documentResult.Errors);
        }

        var document = documentResult.Value;

        // Add this UserId to the list of shared UserIds.
        var operationResult = document.ShareWithUser(new UserId(request.UserId));
        if (operationResult.IsFailed)
        {
            return Result.Fail(operationResult.Errors);
        }

        // Update the Document Repository
        var updateResult = await documentRepository.UpdateAsync(document);
        if (updateResult.IsFailed)
        {
            return Result.Fail(updateResult.Errors);
        }

        return Result.Ok();
    }
}
