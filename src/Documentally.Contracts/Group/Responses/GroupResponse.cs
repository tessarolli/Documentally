// <copyright file="GroupResponse.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Group.Responses;

/// <summary>
/// The Contract for Group Response.
/// Contract for a Group Aggregate Instance Response.
/// Can be used in lists.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record GroupResponse(
    long Id,
    string Name,
    long OwnerId,
    List<long> MembersIds,
    DateTime CreatedAtUtc);
