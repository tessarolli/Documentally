// <copyright file="ShareDocumentWithUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
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
        var updateResult = await documentRepository.ShareDocumentWithUserAsync(new DocId(request.DocumentId), request.UserId);
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
