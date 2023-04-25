// <copyright file="GetDocumentByIdQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.Documents.Queries.GetDocumentById;

/// <summary>
/// Validator.
/// </summary>
public class GetDocumentByIdQueryValidator : AbstractValidator<GetDocumentByIdQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDocumentByIdQueryValidator"/> class.
    /// </summary>
    public GetDocumentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
         .NotNull()
         .GreaterThan(0)
         .WithMessage("Document Id is required");
    }
}
