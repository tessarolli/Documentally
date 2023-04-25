// <copyright file="ShareDocumentWithGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
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
        var updateResult = await documentRepository.ShareDocumentWithGroupAsync(new DocId(request.DocumentId), request.GroupId);
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
