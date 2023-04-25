// <copyright file="ShareDocumentWithGroupRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Document.Requests;

/// <summary>
/// Request to Share Document With Group
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record ShareDocumentWithGroupRequest(
    long DocumentId,
    long GroupId);
