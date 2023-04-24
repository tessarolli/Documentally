// <copyright file="UploadDocumentCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Documents.Results;
using Microsoft.AspNetCore.Http;

namespace Documentally.Application.Users.Commands.UploadDocument;

/// <summary>
/// Command to add an Document into the repository and Upload it the Cloud Blob Storage Provider.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UploadDocumentCommand(
    long UserId,
    string FileName,
    string? Description,
    string? Category,
    IFormFile File) : ICommand<DocumentResult>;
