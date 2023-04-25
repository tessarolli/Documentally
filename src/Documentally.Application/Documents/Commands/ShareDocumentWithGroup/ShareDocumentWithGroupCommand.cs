// <copyright file="ShareDocumentWithGroupCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithGroup;

/// <summary>
/// Share Document With Group Command.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record ShareDocumentWithGroupCommand(
    long DocumentId,
    long GroupId) : ICommand;
