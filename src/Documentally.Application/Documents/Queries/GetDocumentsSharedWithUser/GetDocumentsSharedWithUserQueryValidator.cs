// <copyright file="GetDocumentsSharedWithUserQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentValidation;

namespace Documentally.Application.UserDocuments.Queries.GetUserDocumentssList;

/// <summary>
/// Validator.
/// </summary>
public class GetDocumentsSharedWithUserQueryValidator : AbstractValidator<GetDocumentsSharedWithUserQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDocumentsSharedWithUserQueryValidator"/> class.
    /// </summary>
    public GetDocumentsSharedWithUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User Id is required");
    }
}
