// <copyright file="ConflictError.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;

namespace Documentally.Application.Common.Errors;

/// <summary>
/// Not Found Error.
/// </summary>
public class ConflictError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public ConflictError(string message = "A resource with the same content already exists.")
        : base(message)
    {
    }
}
