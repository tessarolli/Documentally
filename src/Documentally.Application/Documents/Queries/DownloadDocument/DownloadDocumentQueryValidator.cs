// <copyright file="DownloadDocumentQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Queries.DownloadDocumentQuery;

/// <summary>
/// Validator.
/// </summary>
public class DownloadDocumentQueryValidator : AbstractValidator<DownloadDocumentQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DownloadDocumentQueryValidator"/> class.
    /// </summary>
    public DownloadDocumentQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Document Id is required");
    }
}
