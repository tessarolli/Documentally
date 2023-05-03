// <copyright file="DocumentAccessDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// Document Access Data Transfer Object
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record DocumentAccessDto(
    long id,
    long document_id,
    long? user_id,
    long? group_id);
