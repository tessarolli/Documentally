// <copyright file="RemoveUserFromGroupCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Groups.Results;

namespace Documentally.Application.Groups.Commands.RemoveUserFromGroup;

/// <summary>
/// Gets the List of Groups.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record RemoveUserFromGroupCommand(
    long GroupId,
    long UserId) : ICommand<GroupResult>;
