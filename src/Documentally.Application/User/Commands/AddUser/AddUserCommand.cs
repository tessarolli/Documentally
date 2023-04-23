﻿// <copyright file="AddUserCommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.User.Results;

namespace Documentally.Application.User.Commands.AddUser;

/// <summary>
/// Command to Add an User into the repository.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = ".")]
public record AddUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<UserResult>;
