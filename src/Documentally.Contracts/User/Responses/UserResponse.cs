// <copyright file="UserResponse.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.User.Responses;

/// <summary>
/// The Contract for User Response.
/// Contract for a User Aggregate Instance Response.
/// Can be used in lists.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UserResponse(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role,
    DateTime CreatedAtUtc);
