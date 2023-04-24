// <copyright file="DeleteGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Groups.Commands.DeleteGroup;

/// <summary>
/// Validator.
/// </summary>
public class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteGroupCommandValidator"/> class.
    /// </summary>
    public DeleteGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Group Id cannot be empty");
    }
}
