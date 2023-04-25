// <copyright file="GetUserDocumentsRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Documentally.Contract.Documents.Requests;

/// <summary>
/// The Contract for GetUserDocumentsing a Document Request.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record GetUserDocumentsRequest(
    long UserId);
