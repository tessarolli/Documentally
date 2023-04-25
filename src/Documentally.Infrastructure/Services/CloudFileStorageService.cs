// <copyright file="CloudFileStorageService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Azure.Storage.Blobs;
using Documentally.Application.Abstractions.Services;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Documentally.Infrastructure.Services;

/// <summary>
/// Cloud File Storage Service on Azure Blobs.
/// </summary>
public class CloudFileStorageService : ICloudFileStorageService
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly string containerName = "documentally";

    /// <summary>
    /// Initializes a new instance of the <see cref="CloudFileStorageService"/> class.
    /// </summary>
    /// <param name="blobServiceClient">The Azure Blob Services being injected.</param>
    public CloudFileStorageService(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
    }

    /// <inheritdoc/>
    public async Task<Result<(string, string)>> UploadFileAsync(IFormFile formFile)
    {
        try
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // 1. Get the File Extension
            var extension = Path.GetExtension(formFile.FileName);
            BlobClient blobClient;
            string newFileName;

            // Generate a Unique File Name in the format of Guid.extension.
            do
            {
                newFileName = Guid.NewGuid().ToString() + extension;

                blobClient = containerClient.GetBlobClient(newFileName);
            }
            while (await blobClient.ExistsAsync());

            // Upload the File with the updated name to the Cloud Provider
            await blobClient.UploadAsync(formFile.OpenReadStream(), true);

            // Returns the URI for accessing the file
            return Result.Ok((newFileName, blobClient.Uri.ToString()));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<Stream>> DownloadFileAsync(string fileName)
    {
        try
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var response = await blobClient.DownloadAsync();

            return Result.Ok(response.Value.Content);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteFileAsync(string fileName)
    {
        try
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var deleteResult = await blobClient.DeleteIfExistsAsync();
            if (deleteResult)
            {
                return Result.Ok();
            }

            return Result.Fail("We couldnt delete the file from the cloud storage.");
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}