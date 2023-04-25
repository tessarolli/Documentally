// <copyright file="UpdateDocumentCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Commands.UpdateDocument;

/// <summary>
/// Validator.
/// </summary>
public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDocumentCommandValidator"/> class.
    /// </summary>
    public UpdateDocumentCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("Document Name cannot be empty")
            .MaximumLength(255)
            .WithMessage("Document name cannot be larger than 255 characteres");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Document User Id is required");
    }
}
