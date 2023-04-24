// <copyright file="GetGroupsListQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Groups.Queries.GetGroupsList;

/// <summary>
/// Validator.
/// </summary>
public class GetGroupsListQueryValidator : AbstractValidator<GetGroupsListQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupsListQueryValidator"/> class.
    /// </summary>
    public GetGroupsListQueryValidator()
    {
    }
}
