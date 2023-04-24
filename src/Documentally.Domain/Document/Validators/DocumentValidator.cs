// <copyright file="DocumentValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common;
using Documentally.Domain.Doc.ValueObjects;
using FluentValidation;

namespace Documentally.Domain.Document.Validators;

/// <summary>
/// Document Entity Validation Rules.
/// </summary>
public sealed class DocumentValidator : EntityValidator<Document, DocId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentValidator"/> class.
    /// </summary>
    public DocumentValidator()
    {
        RuleFor(g => ((Document)g).Name)
            .NotEmpty()
            .WithMessage("Document Name is required.")
            .MaximumLength(250)
            .WithMessage("Document Name must be at most 250 characters long.");

        RuleFor(g => ((Document)g).OwnerId)
            .NotNull()
            .WithMessage("Document Owner is required.");

        RuleFor(g => ((Document)g).BlobUrl)
            .NotEmpty()
            .WithMessage("Document's Blob Url is required.");
    }
}
