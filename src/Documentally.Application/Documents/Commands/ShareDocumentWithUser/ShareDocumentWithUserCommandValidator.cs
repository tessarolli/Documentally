// <copyright file="ShareDocumentWithUserCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithUser;

/// <summary>
/// Validator.
/// </summary>
public class ShareDocumentWithUserCommandValidator : AbstractValidator<ShareDocumentWithUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShareDocumentWithUserCommandValidator"/> class.
    /// </summary>
    public ShareDocumentWithUserCommandValidator()
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
