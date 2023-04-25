// <copyright file="DownloadDocumentQuery.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Documents.Results;

namespace Documentally.Application.Documents.Queries.DownloadDocumentQuery;

/// <summary>
/// Gets the Document.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record DownloadDocumentQuery(
    long Id) : IQuery<(Stream, string)>;
