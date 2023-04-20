// <copyright file="ResultControllerBase.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Validation.Errors;
using Documentally.Domain.BaseClasses;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Common.Controllers;

/// <summary>
/// Base Class for Api Controller that Handles Validation Results.
/// </summary>
public class ResultControllerBase : ControllerBase
{
    /// <summary>
    /// Validates the result.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="result">FluentResult.</param>
    /// <param name="success">Action to perform when success.</param>
    /// <param name="failure">Action to perform when failure.</param>
    /// <returns>ActionResult according to Result.</returns>
    [NonAction]
    public ActionResult ValidateResult<T>(Result<T> result, Func<ActionResult> success, Action failure)
    {
        if (!result.IsSuccess)
        {
            return success.Invoke();
        }

        failure.Invoke();

        var errorMessages = result.Errors.Aggregate(string.Empty, (current, next) => $"{current}{next}\n");

        if (result.HasError<ValidationError>())
        {
            return BadRequest(new ProblemDetails
            {
                Instance = HttpContext.Request.Path,
                Title = "Bad Request",
                Status = 400,
                Type = "https://httpstatuses.com/400",
                Detail = errorMessages,
            });
        }

        if (result.HasError<UnauthorizedError>())
        {
            return StatusCode(403, new ProblemDetails
            {
                Instance = HttpContext.Request.Path,
                Title = "Forbidden",
                Status = 403,
                Type = "https://httpstatuses.com/403",
                Detail = errorMessages,
            });
        }

        if (result.HasError<NotFoundError>())
        {
            return NotFound(new ProblemDetails
            {
                Instance = HttpContext.Request.Path,
                Title = "Not Found",
                Status = 404,
                Type = "https://httpstatuses.com/404",
                Detail = errorMessages,
            });
        }

        return StatusCode(500, new ProblemDetails
        {
            Instance = HttpContext.Request.Path,
            Title = "Internal Server Error",
            Status = 500,
            Type = "https://httpstatuses.com/500",
            Detail = errorMessages,
        });
    }
}
