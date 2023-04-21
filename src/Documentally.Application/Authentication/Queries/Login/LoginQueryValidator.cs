// <copyright file="LoginQueryValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Authentication.Queries.Login;
using FluentValidation;

namespace Documentally.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Validation Rules for the Login Query.
    /// </summary>
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginQueryValidator"/> class.
        /// </summary>
        public LoginQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}