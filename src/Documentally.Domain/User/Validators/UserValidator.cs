// <copyright file="UserValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common;
using Documentally.Domain.User.ValueObjects;
using FluentValidation;

namespace Documentally.Domain.User.Validators;

/// <summary>
/// User Entity Validation Rules.
/// </summary>
public sealed class UserValidator : EntityValidator<User, UserId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserValidator"/> class.
    /// </summary>
    public UserValidator()
    {
        RuleFor(g => ((User)g).FirstName)
            .NotEmpty()
            .WithMessage("The First Name is required.")
            .MaximumLength(50)
            .WithMessage("The First Name must be at most 50 characters long.");

        RuleFor(g => ((User)g).LastName)
            .NotEmpty()
            .WithMessage("The Last Name is required.")
            .MaximumLength(50)
            .WithMessage("The Last Name must be at most 50 characters long.");

        RuleFor(g => ((User)g).Email)
            .EmailAddress()
            .WithMessage("Is not a valid e-mail address.");
    }
}
