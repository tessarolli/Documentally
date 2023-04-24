// <copyright file="UserDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Enums;

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// User's Data Transfer Object.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record UserDto(
    long id,
    string first_name,
    string last_name,
    string email,
    string password,
    Roles role,
    DateTime created_at_utc);
