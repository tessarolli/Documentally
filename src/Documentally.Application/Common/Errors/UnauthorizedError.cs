// <copyright file="UnauthorizedError.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;

namespace Documentally.Application.Common.Errors;

/// <summary>
/// Unauthorized Error.
/// </summary>
public class UnauthorizedError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedError"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public UnauthorizedError(string message = "You dont have permission to access this resource.")
        : base(message)
    {
    }
}
