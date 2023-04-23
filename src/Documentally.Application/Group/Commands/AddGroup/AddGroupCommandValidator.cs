// <copyright file="AddGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Group.Commands.AddGroup;

/// <summary>
/// Validator.
/// </summary>
public class AddGroupCommandValidator : AbstractValidator<AddGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddGroupCommandValidator"/> class.
    /// </summary>
    public AddGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Group Name cannot be empty")
            .MaximumLength(255)
            .WithMessage("Group name cannot be larger than 255 characteres");

        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Group Owner Id is required");
    }
}
