// <copyright file="UploadDocumentCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Documents.Results;
using Documentally.Application.Users.Commands.UploadDocument;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Commands.UploadDocument;

/// <summary>
/// Implementation for the UploadDocumentCommandHandler.
/// </summary>
public class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand, DocumentResult>
{
    private readonly IDocumentRepository documentRepository;
    private readonly ICloudFileStorageService cloudFileStorageService;
    private readonly IAuthenticatedUserService authenticatedUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected DocumentRepository.</param>
    /// <param name="cloudFileStorageService">Injected CloudFileStorageService.</param>
    /// <param name="authenticatedUserService">Injected IAuthenticatedUserService.</param>
    public UploadDocumentCommandHandler(IDocumentRepository documentRepository, ICloudFileStorageService cloudFileStorageService, IAuthenticatedUserService authenticatedUserService)
    {
        this.documentRepository = documentRepository;
        this.cloudFileStorageService = cloudFileStorageService;
        this.authenticatedUserService = authenticatedUserService;
    }

    /// <inheritdoc/>
    public async Task<Result<DocumentResult>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var authenticatedUser = await authenticatedUserService.GetAuthenticatedUserAsync();
        if (authenticatedUser is null)
        {
            return Result.Fail("Please Login Again!");
        }

        // 1. Upload the File to the Cloud Storage.
        var uploadResult = await cloudFileStorageService.UploadFileAsync(request.File);
        if (uploadResult.IsFailed)
        {
            return Result.Fail(uploadResult.Errors);
        }

        // 2. Create the Document Aggregate Instance.
        var documentResult = Domain.Document.Document.Create(
                   null,
                   new UserId(authenticatedUser.Id.Value),
                   request.FileName,
                   request.Description,
                   request.Category,
                   request.File.Length,
                   uploadResult.Value.Item2,
                   uploadResult.Value.Item1);

        // 3. Persist the Document Aggregate in the Document Repository
        if (documentResult.IsSuccess)
        {
            var addResult = await documentRepository.AddAsync(documentResult.Value);
            if (addResult.IsSuccess)
            {
                var doc = new DocumentResult(
                    addResult.Value.Id.Value,
                    addResult.Value.OwnerId.Value,
                    addResult.Value.Name,
                    addResult.Value.Description,
                    addResult.Value.Category,
                    addResult.Value.Size,
                    addResult.Value.BlobUrl,
                    addResult.Value.CloudFileName,
                    addResult.Value.SharedGroupIds.Select(x => x.Value).ToList(),
                    addResult.Value.SharedUserIds.Select(x => x.Value).ToList(),
                    addResult.Value.PostedAtUtc);

                // 4. Return the success result reponse.
                return doc;
            }
            else
            {
                // in case of error delete the uploaded file from the cloud storage.
                var deleteResult = await cloudFileStorageService.DeleteFileAsync(uploadResult.Value.Item1);
                if (deleteResult.IsFailed)
                {
                    return Result.Fail("An error ocurred when we were adding this document to the repository, but it failed, so we have to delete the file from the cloud storage, and then this deletion also failed.");
                }

                return Result.Fail(addResult.Errors);
            }
        }
        else
        {
            return Result.Fail(documentResult.Errors);
        }
    }
}
