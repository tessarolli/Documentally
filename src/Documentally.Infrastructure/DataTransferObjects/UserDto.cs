// <copyright file="UserDto.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Enums;

namespace Documentally.Infrastructure.DataTransferObjects;

/// <summary>
/// User's Data Transfer Object.
/// </summary>
/// <param name="id">User Id</param>
/// <param name="first_name">User First Name</param>
/// <param name="last_name">User Last Name</param>
/// <param name="email">User E-Mail</param>
/// <param name="password">User Password</param>
/// <param name="role">User Role</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = ".")]
public record UserDto(
    long id,
    string first_name,
    string last_name,
    string email,
    string password,
    Roles role);
