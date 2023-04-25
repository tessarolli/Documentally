// <copyright file="UnshareDocumentWithGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Commands.UnshareDocumentWithGroup;

/// <summary>
/// Validator.
/// </summary>
public class UnshareDocumentWithGroupCommandValidator : AbstractValidator<UnshareDocumentWithGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnshareDocumentWithGroupCommandValidator"/> class.
    /// </summary>
    public UnshareDocumentWithGroupCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Document Id is required");

        RuleFor(x => x.GroupId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Group Id is required");
    }
}
