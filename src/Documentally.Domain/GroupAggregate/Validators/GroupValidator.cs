// <copyright file="GroupValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common;
using Documentally.Domain.GroupAggregate.ValueObjects;
using FluentValidation;

namespace Documentally.Domain.GroupAggregate.Validators;

/// <summary>
/// Group Entity Validation Rules.
/// </summary>
public sealed class GroupValidator : EntityValidator<Group, GroupId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupValidator"/> class.
    /// </summary>
    public GroupValidator()
    {
        RuleFor(g => ((Group)g).Name)
            .NotEmpty()
            .WithMessage("The group name is required.");

        RuleFor(g => ((Group)g).Name)
            .MaximumLength(255)
            .WithMessage("The group name must be at most 255 characters long.");
    }
}
