// <copyright file="DeleteDocumentCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Documents.Results;
using Microsoft.AspNetCore.Http;

namespace Documentally.Application.Users.Commands.DeleteDocument;

/// <summary>
/// Command to add an Document into the repository and Delete it the Cloud Blob Storage Provider.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record DeleteDocumentCommand(
    long DocumentId) : ICommand;
