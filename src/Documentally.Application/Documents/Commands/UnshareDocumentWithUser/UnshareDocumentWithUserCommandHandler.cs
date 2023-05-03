// <copyright file="UnshareDocumentWithUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.UnshareDocumentWithUser;

/// <summary>
/// Implementation for the UnshareDocumentWithUserCommand.
/// </summary>
public class UnshareDocumentWithUserCommandHandler : ICommandHandler<UnshareDocumentWithUserCommand>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnshareDocumentWithUserCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected UserRepository.</param>
    public UnshareDocumentWithUserCommandHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(UnshareDocumentWithUserCommand request, CancellationToken cancellationToken)
    {
        // Gets the Document by its Id.
        var documentResult = await documentRepository.GetByIdAsync(new DocId(request.DocumentId));
        if (documentResult.IsFailed)
        {
            return Result.Fail(documentResult.Errors);
        }

        var document = documentResult.Value;

        // Remove this UserId from the list of shared UserIds.
        var operationResult = document.UnshareWithUser(new UserId(request.UserId));
        if (operationResult.IsFailed)
        {
            return Result.Fail(documentResult.Errors);
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
