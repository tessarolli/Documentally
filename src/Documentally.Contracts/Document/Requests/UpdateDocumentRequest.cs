// <copyright file="UpdateDocumentRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contract.Documents.Requests;

/// <summary>
/// The Contract for Updating a Document Request.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UpdateDocumentRequest(
    long Id,
    long UserId,
    string FileName,
    string? Description,
    string? Category,
    long Size,
    string CloudFileName,
    string BlobUrl);
