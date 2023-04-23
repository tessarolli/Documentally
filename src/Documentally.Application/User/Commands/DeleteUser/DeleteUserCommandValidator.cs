// <copyright file="DeleteUserCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.User.Commands.DeleteUser;

/// <summary>
/// Validator.
/// </summary>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserCommandValidator"/> class.
    /// </summary>
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User Id cannot be empty");
    }
}
