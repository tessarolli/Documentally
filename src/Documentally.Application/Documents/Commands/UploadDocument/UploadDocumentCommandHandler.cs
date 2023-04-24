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
using Microsoft.AspNetCore.Server.HttpSys;

namespace Documentally.Application.Documents.Commands.UploadDocument;

/// <summary>
/// Implementation for the UploadDocumentCommandHandler.
/// </summary>
public class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand, DocumentResult>
{
    private readonly IDocumentRepository documentRepository;
    private readonly ICloudFileStorageService cloudFileStorageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected DocumentRepository.</param>
    /// <param name="cloudFileStorageService">Injected CloudFileStorageService.</param>
    public UploadDocumentCommandHandler(IDocumentRepository documentRepository, ICloudFileStorageService cloudFileStorageService)
    {
        this.documentRepository = documentRepository;
        this.cloudFileStorageService = cloudFileStorageService;
    }

    /// <inheritdoc/>
    public async Task<Result<DocumentResult>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        // 1. Upload the File to the Cloud Storage.
        var uploadResult = await cloudFileStorageService.UploadFileAsync(request.File);
        if (uploadResult.IsFailed)
        {
            return Result.Fail(uploadResult.Errors);
        }

        // 2. Create the Document Aggregate Instance.
        var documentResult = Domain.Document.Document.Create(
                   null,
                   new UserId(request.UserId),
                   request.FileName,
                   request.Description,
                   request.Category,
                   request.File.Length,
                   string.Empty);

        // 3. Persist the Document Aggregate in the Document Repository
        if (documentResult.IsSuccess)
        {
            return Result.Ok();

            //var addResult = await documentRepository.AddAsync(documentResult.Value);
            //if (addResult.IsSuccess)
            //{
            //    return Result.Ok(new DocumentResult(
            //        addResult.Value.Id.Value,
            //        addResult.Value.FirstName,
            //        addResult.Value.LastName,
            //        addResult.Value.Email,
            //        addResult.Value.Password.HashedPassword,
            //        (int)addResult.Value.Role,
            //        addResult.Value.CreatedAtUtc));
            //}
            //else
            //{
            //    return Result.Fail(addResult.Errors);
            //}
        }
        else
        {
            return Result.Fail(documentResult.Errors);
        }
    }
}
