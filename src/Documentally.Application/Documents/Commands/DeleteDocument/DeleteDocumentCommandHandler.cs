// <copyright file="DeleteDocumentCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Common.Errors;
using Documentally.Application.Users.Commands.DeleteDocument;
using Documentally.Domain.Doc.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.DeleteDocument;

/// <summary>
/// Implementation for the DeleteDocumentCommandHandler.
/// </summary>
public class DeleteDocumentCommandHandler : ICommandHandler<DeleteDocumentCommand>
{
    private readonly IDocumentRepository documentRepository;
    private readonly ICloudFileStorageService cloudFileStorageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected DocumentRepository.</param>
    /// <param name="cloudFileStorageService">Injected CloudFileStorageService.</param>
    public DeleteDocumentCommandHandler(IDocumentRepository documentRepository, ICloudFileStorageService cloudFileStorageService)
    {
        this.documentRepository = documentRepository;
        this.cloudFileStorageService = cloudFileStorageService;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        // 1. Loads the document from the repository
        var loadDocResult = await documentRepository.GetByIdAsync(new DocId(request.DocumentId));
        if (loadDocResult.IsFailed)
        {
            return Result.Fail(new NotFoundError());
        }

        var doc = loadDocResult.Value;

        // 2. Delete the File from the Cloud Storage.
        var deleteCloudFileResult = await cloudFileStorageService.DeleteFileAsync(doc.CloudFileName);
        if (deleteCloudFileResult.IsFailed)
        {
            return Result.Fail(deleteCloudFileResult.Errors);
        }

        // 3. Proceed to delete this document from the document repository
        var deleteResult = await documentRepository.RemoveAsync(doc.Id);
        if (deleteResult.IsSuccess)
        {
            return Result.Ok();
        }

        return Result.Fail(deleteResult.Errors);
    }
}
