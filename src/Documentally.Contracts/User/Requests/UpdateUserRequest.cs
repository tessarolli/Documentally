// <copyright file="UpdateUserRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.User.Requests;

/// <summary>
/// A request to update the User in the repository.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UpdateUserRequest(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role);