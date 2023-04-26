// <copyright file="IExceptionHandlingService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using Microsoft.Extensions.Logging;

namespace Documentally.Application.Abstractions.Services;

/// <summary>
/// Exception Handling Interface.
/// </summary>
public interface IExceptionHandlingService
{
    /// <summary>
    /// Handles the exception and returns a informational message.
    /// </summary>
    /// <param name="exception">Exception to be handled.</param>
    /// <param name="logger">Logger instance for logging the exception.</param>
    /// <returns>A result with the errors from the exception.</returns>
    Result HandleException(Exception exception, ILogger? logger = null);
}
