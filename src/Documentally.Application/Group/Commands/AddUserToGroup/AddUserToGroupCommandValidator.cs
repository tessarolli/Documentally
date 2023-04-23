// <copyright file="AddUserToGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Group.Commands.AddUserToGroup;

/// <summary>
/// Validator.
/// </summary>
public class AddUserToGroupCommandValidator : AbstractValidator<AddUserToGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserToGroupCommandValidator"/> class.
    /// </summary>
    public AddUserToGroupCommandValidator()
    {
        RuleFor(x => x.GroupId)
             .NotEmpty()
             .GreaterThan(0)
             .WithMessage("Group Id is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User Id is required");
    }
}
