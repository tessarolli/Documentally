// <copyright file="GetUserByIdQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.User.Queries.GetUserById;

/// <summary>
/// Validator.
/// </summary>
public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryValidator"/> class.
    /// </summary>
    public GetUserByIdQueryValidator()
    {
    }
}
