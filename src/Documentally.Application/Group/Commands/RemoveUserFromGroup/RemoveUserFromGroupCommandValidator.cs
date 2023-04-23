// <copyright file="RemoveUserFromGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Group.Commands.RemoveUserFromGroup;

/// <summary>
/// Validator.
/// </summary>
public class RemoveUserFromGroupCommandValidator : AbstractValidator<RemoveUserFromGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveUserFromGroupCommandValidator"/> class.
    /// </summary>
    public RemoveUserFromGroupCommandValidator()
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
