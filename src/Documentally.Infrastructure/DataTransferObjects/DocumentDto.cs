// <copyright file="DocumentDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// Document Data Transfer Object
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record DocumentDto(
    long id,
    long owner_id,
    string doc_name,
    string? doc_description,
    string? doc_category,
    long doc_size,
    string blob_url,
    DateTime posted_date_utc);
