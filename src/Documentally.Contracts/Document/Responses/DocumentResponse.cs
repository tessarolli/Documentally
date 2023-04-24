// <copyright file="DocumentResponse.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Document.Responses;

/// <summary>
/// The Contract for Document Response.
/// Contract for a Document Aggregate Instance Response.
/// Can be used in lists.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record DocumentResponse(
    long Id,
    string Name,
    string? Description,
    string? Category,
    long Size,
    string BlobUrl,
    DateTime PostedAtUtc);