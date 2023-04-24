// <copyright file="GetGroupByIdQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Groups.Queries.GetGroupById;

/// <summary>
/// Validator.
/// </summary>
public class GetGroupByIdQueryValidator : AbstractValidator<GetGroupByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupByIdQueryValidator"/> class.
    /// </summary>
    public GetGroupByIdQueryValidator()
    {
    }
}
