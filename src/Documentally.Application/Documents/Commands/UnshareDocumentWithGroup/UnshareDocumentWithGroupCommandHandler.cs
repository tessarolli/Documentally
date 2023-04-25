// <copyright file="UnshareDocumentWithGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Doc.ValueObjects;
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
        var updateResult = await documentRepository.RemoveDocumentShareFromGroupAsync(new DocId(request.DocumentId), request.GroupId);
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
