﻿// <copyright file="GetUsersListQuery.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.User.Results;

namespace Documentally.Application.User.Queries.GetUsersList;

/// <summary>
/// Gets the List of Users.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
public record GetUsersListQuery() : IQuery<List<UserResult>>;
