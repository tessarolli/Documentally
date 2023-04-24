// <copyright file="ICloudFileStorageService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using Microsoft.AspNetCore.Http;

namespace Documentally.Application.Abstractions.Services;

/// <summary>
/// Interface for Cloud File Storage Services.
/// </summary>
public interface ICloudFileStorageService
{
    /// <summary>
    /// Uploads a File to the File Storage Cloud Provider.
    /// </summary>
    /// <param name="formFile">The IFormFile for uploading.</param>
    /// <returns>A Url string for the uploaded file location.</returns>
    Task<Result<string>> UploadFileAsync(IFormFile formFile);

    /// <summary>
    /// Downloads a File from the Cloud Provider.
    /// </summary>
    /// <param name="fileName">The File Name.</param>
    /// <returns>A Stream for downloading the file.</returns>
    Task<Result<Stream>> DownloadFileAsync(string fileName);

    /// <summary>
    /// Deletes a File from the Cloud Provider.
    /// </summary>
    /// <param name="fileName">The File name.</param>
    /// <returns>Awaitable Task.</returns>
    Task<Result> DeleteFileAsync(string fileName);
}
