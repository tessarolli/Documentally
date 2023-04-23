// <copyright file="GetUsersListQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.User.Queries.GetUsersList;

/// <summary>
/// Validator.
/// </summary>
public class GetUsersListQueryValidator : AbstractValidator<GetUsersListQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersListQueryValidator"/> class.
    /// </summary>
    public GetUsersListQueryValidator()
    {
    }
}
