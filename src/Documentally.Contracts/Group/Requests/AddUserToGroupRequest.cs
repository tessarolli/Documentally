// <copyright file="AddUserToGroupRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Group.Requests;

/// <summary>
/// A request to add an user to a group in the repository.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record AddUserToGroupRequest(
    long GroupId,
    long UserId);
