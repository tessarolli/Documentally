// <copyright file="GroupDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// Group Data Transfer Object
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record GroupDto(
    long id,
    string name,
    long owner_id,
    DateTime created_at_utc);
