// <copyright file="GetGroupsListQuery.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Group.Results;

namespace Documentally.Application.Group.Queries.GetGroupsList;

/// <summary>
/// Gets the List of Groups.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = ".")]
public record GetGroupsListQuery() : IQuery<List<GroupResult>>;
