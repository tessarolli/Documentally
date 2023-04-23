// <copyright file="GroupMemberDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// Group Member Data Transfer Object.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record GroupMemberDto(
    long group_id,
    long user_id);
