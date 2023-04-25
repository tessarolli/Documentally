// <copyright file="UnshareDocumentWithUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
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
        var updateResult = await documentRepository.RemoveDocumentShareFromUserAsync(new DocId(request.DocumentId), request.UserId);
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
