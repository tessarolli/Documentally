// <copyright file="DeleteUserRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.User.Requests;

/// <summary>
/// A request to delete a User from the repository.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record DeleteUserRequest(
    long Id);
