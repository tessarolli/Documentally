// <copyright file="ShareDocumentWithUserCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithUser;

/// <summary>
/// Share Document With User Command.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record ShareDocumentWithUserCommand(
    long DocumentId,
    long UserId) : ICommand;
