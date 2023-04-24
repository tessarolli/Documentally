// <copyright file="UserResult.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Application.Users.Results;

/// <summary>
/// Contract for the User response.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UserResult(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role,
    DateTime CreatedAtUtc);