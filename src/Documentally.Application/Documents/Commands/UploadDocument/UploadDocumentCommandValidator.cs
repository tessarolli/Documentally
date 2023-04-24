// <copyright file="UploadDocumentCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Users.Commands.UploadDocument;

/// <summary>
/// Validator.
/// </summary>
public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadDocumentCommandValidator"/> class.
    /// </summary>
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("First Name cannot be larger than 50 characteres");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File Name cannot be empty")
            .MaximumLength(250)
            .WithMessage("File Name cannot be larger than 250 characteres");

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("The File Is Required");

    }
}
