// <copyright file="GetUserDocumentsListQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.UserDocuments.Queries.GetUserDocumentssList;

/// <summary>
/// Validator.
/// </summary>
public class GetUserDocumentsListQueryValidator : AbstractValidator<GetUserDocumentsListQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserDocumentsListQueryValidator"/> class.
    /// </summary>
    public GetUserDocumentsListQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User Id is required");
    }
}
