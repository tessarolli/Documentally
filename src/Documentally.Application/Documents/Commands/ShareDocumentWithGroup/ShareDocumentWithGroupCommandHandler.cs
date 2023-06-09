﻿// <copyright file="ShareDocumentWithGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Group.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithGroup;

/// <summary>
/// Implementation for the ShareDocumentWithGroupCommand.
/// </summary>
public class ShareDocumentWithGroupCommandHandler : ICommandHandler<ShareDocumentWithGroupCommand>
{
    private readonly IDocumentRepository documentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareDocumentWithGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected GroupRepository.</param>
    public ShareDocumentWithGroupCommandHandler(IDocumentRepository documentRepository)
    {
        this.documentRepository = documentRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(ShareDocumentWithGroupCommand request, CancellationToken cancellationToken)
    {
        // Gets the Document by its Id.
        var documentResult = await documentRepository.GetByIdAsync(new DocId(request.DocumentId));
        if (documentResult.IsFailed)
        {
            return Result.Fail(documentResult.Errors);
        }

        var document = documentResult.Value;

        // Add this GroupId to the list of shared GroupIds.
        var operationResult = document.ShareWithGroup(new GroupId(request.GroupId));
        if (operationResult.IsFailed)
        {
            return Result.Fail(operationResult.Errors);
        }

        // Update the Document Repository
        var updateResult = await documentRepository.UpdateAsync(document);
        if (updateResult.IsSuccess)
        {
            return Result.Ok();
        }
        else
        {
            return Result.Fail(updateResult.Errors);
        }
    }
}
