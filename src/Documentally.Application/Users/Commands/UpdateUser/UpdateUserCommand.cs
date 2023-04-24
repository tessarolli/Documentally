// <copyright file="UpdateUserCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Users.Results;

namespace Documentally.Application.Users.Commands.UpdateUser;

/// <summary>
/// Gets the List of Users.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record UpdateUserCommand(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role) : ICommand<UserResult>;
