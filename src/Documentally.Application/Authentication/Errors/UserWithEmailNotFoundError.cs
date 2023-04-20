// <copyright file="UserWithEmailNotFoundError.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Validation.Errors;

namespace Documentally.Application.Authentication.Errors;

/// <summary>
/// User with informed email does not exists.
/// </summary>
public class UserWithEmailNotFoundError : UnauthorizedError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithEmailNotFoundError"/> class.
    /// </summary>
    public UserWithEmailNotFoundError()
    {
        Message = "Account with given e-mail address does not exist";
    }
}
