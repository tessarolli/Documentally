// <copyright file="UnshareDocumentWithGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.UnshareDocumentWithGroup;

/// <summary>
/// Implementation for the UnshareDocumentWithGroupCommand.
/// </summary>
public class UnshareDocumentWithGroupCommandHandler : ICommandHandler<UnshareDocumentWithGroupCommand>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnshareDocumentWithGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected GroupRepository.</param>
    public UnshareDocumentWithGroupCommandHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(UnshareDocumentWithGroupCommand request, CancellationToken cancellationToken)
    {
        // Gets the Document by its Id.
        var documentResult = await documentRepository.GetByIdAsync(new DocId(request.DocumentId));
        if (documentResult.IsFailed)
        {
            return Result.Fail(documentResult.Errors);
        }

        var document = documentResult.Value;

        // Remove this GroupId from the list of shared GroupIds.
        var operationResult = document.UnshareWithGroup(new GroupId(request.GroupId));
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
