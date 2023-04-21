// <copyright file="UserWithEmailAlreadyExistsError.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Errors;

namespace Documentally.Application.Authentication.Errors;

/// <summary>
/// User with this email already exists.
/// </summary>
public class UserWithEmailAlreadyExistsError : ConflictError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithEmailAlreadyExistsError"/> class.
    /// </summary>
    public UserWithEmailAlreadyExistsError()
    {
        Message = "Given E-Mail address is already in use";
    }
}
