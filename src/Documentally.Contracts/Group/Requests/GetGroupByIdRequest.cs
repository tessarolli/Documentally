// <copyright file="GetGroupByIdRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Group.Requests;

/// <summary>
/// A request to fetch a group by its Id.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record GetGroupByIdRequest(
    long Id);
