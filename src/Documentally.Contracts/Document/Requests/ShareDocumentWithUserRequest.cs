// <copyright file="ShareDocumentWithUserRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Document.Requests;

/// <summary>
/// Request to Share Document With User
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record ShareDocumentWithUserRequest(
    long DocumentId,
    long UserId);
