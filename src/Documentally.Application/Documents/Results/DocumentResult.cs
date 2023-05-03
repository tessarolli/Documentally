// <copyright file="DocumentResult.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Application.Documents.Results;

/// <summary>
/// The Document Result.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record DocumentResult(
    long Id,
    long OwnerId,
    string Name,
    string? Description,
    string? Category,
    long Size,
    string BlobUrl,
    string CloudFileName,
    List<long> SharedGroupIds,
    List<long> SharedUserIds,
    DateTime PostedAtUtc);