// <copyright file="InvalidPasswordError.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Errors;

namespace Documentally.Application.Authentication.Errors;

/// <summary>
/// Invalid Password error.
/// </summary>
public class InvalidPasswordError : UnauthorizedError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPasswordError"/> class.
    /// </summary>
    public InvalidPasswordError()
    {
        Message = "Invalid Password";
    }
}
