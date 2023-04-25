// <copyright file="ShareDocumentWithGroupCommandValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Commands.ShareDocumentWithGroup;

/// <summary>
/// Validator.
/// </summary>
public class ShareDocumentWithGroupCommandValidator : AbstractValidator<ShareDocumentWithGroupCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShareDocumentWithGroupCommandValidator"/> class.
    /// </summary>
    public ShareDocumentWithGroupCommandValidator()
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
