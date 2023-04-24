// <copyright file="UploadDocumentRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Documentally.Contract.Documents.Requests;

/// <summary>
/// The Contract for Uploading a Document Request.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UploadDocumentRequest(
    long UserId,
    string FileName,
    string? Description,
    string? Category,
    IFormFile File);
