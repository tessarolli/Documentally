// <copyright file="GetUserByIdRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.User.Requests;

/// <summary>
/// A request to fetch a User by its Id.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record GetUserByIdRequest(
    long Id);
