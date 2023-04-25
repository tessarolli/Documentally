// <copyright file="DownloadDocumentQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Documents.Results;
using Documentally.Domain.Doc.ValueObjects;
using FluentResults;

namespace Documentally.Application.Documents.Queries.DownloadDocumentQuery;

/// <summary>
/// Implementation for the DownloadDocumentQuery.
/// </summary>
public class DownloadDocumentQueryHandler : IQueryHandler<DownloadDocumentQuery, (Stream, string)>
{
    private readonly IDocumentRepository documentRepository;
    private readonly ICloudFileStorageService cloudFileStorageService;
    private readonly IMimeTypeMappingService mimeTypeMappingService;
    private readonly IAuthenticatedUserService authenticatedUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DownloadDocumentQueryHandler"/> class.
    /// </summary>
    /// <param name="documentRepository">Injected DocumentRepository.</param>
    /// <param name="cloudFileStorageService">Injected CloudFileStorageService.</param>
    /// <param name="mimeTypeMappingService">Injected MimeTypeMappingService.</param>
    /// <param name="authenticatedUserService">Injected AuthenticatedUserService.</param>
    public DownloadDocumentQueryHandler(IDocumentRepository documentRepository, ICloudFileStorageService cloudFileStorageService, IMimeTypeMappingService mimeTypeMappingService, IAuthenticatedUserService authenticatedUserService)
    {
        this.documentRepository = documentRepository;
        this.cloudFileStorageService = cloudFileStorageService;
        this.mimeTypeMappingService = mimeTypeMappingService;
        this.authenticatedUserService = authenticatedUserService;
    }

    /// <inheritdoc/>
    public async Task<Result<(Stream, string)>> Handle(DownloadDocumentQuery query, CancellationToken cancellationToken)
    {
        // Get the Document Entity Instance from the repository
        var documentResult = await documentRepository.GetByIdAsync(new DocId(query.Id));
        if (documentResult.IsSuccess)
        {
            // Verify if the Authenticated User has access to Download this document
            var authUser = await authenticatedUserService.GetAuthenticatedUserAsync();
            if (authUser is null)
            {
                return Result.Fail("Please login again");
            }

            // If the owner of the file is not the person who is trying to download it.
            if (documentResult.Value.OwnerId != authUser.Id)
            {
                // check if the current user have permission to download this file.
                var permissionResult = await documentRepository.VerifyUserIdHasAccessToDocIdAsync(documentResult.Value.Id, authUser.Id.Value);
                if (permissionResult.IsFailed)
                {
                    // oh. the authenticated user does not have the necessary permission to download this file.
                    return Result.Fail(permissionResult.Errors);
                }
            }

            // Now Get the Stream to download the file from the cloud storage
            var cloudStreamResult = await cloudFileStorageService.DownloadFileAsync(documentResult.Value.CloudFileName);
            if (cloudStreamResult.IsSuccess)
            {
                // Get the Mime Type for this file
                var mimeType = mimeTypeMappingService.GetMimeType(documentResult.Value.Name);

                // Return the Stream for the Controller method to send to the client.
                return (cloudStreamResult.Value, mimeType);
            }

            return Result.Fail(cloudStreamResult.Errors);
        }

        return Result.Fail(documentResult.Errors);
    }
}