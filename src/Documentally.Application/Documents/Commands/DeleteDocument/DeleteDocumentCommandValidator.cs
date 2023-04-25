// <copyright file="DeleteDocumentCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Users.Commands.DeleteDocument;

/// <summary>
/// Validator.
/// </summary>
public class DeleteDocumentCommandValidator : AbstractValidator<DeleteDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteDocumentCommandValidator"/> class.
    /// </summary>
    public DeleteDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotNull()
            .WithMessage("Document Id Is required");
    }
}
