// <copyright file="UnshareDocumentWithUserCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Commands.UnshareDocumentWithUser;

/// <summary>
/// Validator.
/// </summary>
public class UnshareDocumentWithUserCommandValidator : AbstractValidator<UnshareDocumentWithUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnshareDocumentWithUserCommandValidator"/> class.
    /// </summary>
    public UnshareDocumentWithUserCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Document Id is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("User Id is required");
    }
}
