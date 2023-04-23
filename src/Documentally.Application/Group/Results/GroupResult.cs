// <copyright file="GroupResult.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Application.Group.Results;

/// <summary>
/// Contract for the Group response.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record GroupResult(
    long Id,
    string Name,
    long OwnerId,
    IList<long> MembersIds,
    DateTime CreatedAtUtc);