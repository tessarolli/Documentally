// <copyright file="ResultControllerBase.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Errors;
using Documentally.Domain.BaseClasses;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Common.Controllers;

/// <summary>
/// Base Class for Api Controller that Handles Validation Results.
/// </summary>
[ApiController]
[Authorize]
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
        if (result.IsSuccess)
        {
            return success.Invoke();
        }

        failure.Invoke();

        var errorMessages = result.Errors.Select(e => e.Message).ToList();

        var status = GetStatusCode(result);

        var problemDetails = new ProblemDetails
        {
            Instance = HttpContext.Request.Path,
            Status = status.Item1,
            Title = status.Item2,
            Detail = "One or more erros ocurred.",
            Type = $"https://httpstatuses.com/{GetStatusCode(result)}",
            Extensions = { { "errors", errorMessages } },
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
        };
    }

    /// <summary>
    /// Get Http Status Code from Result.
    /// </summary>
    /// <typeparam name="T">Result Type.</typeparam>
    /// <param name="result">instance.</param>
    /// <returns>Http Status code.</returns>
    private static (int, string) GetStatusCode<T>(Result<T> result)
    {
        if (result.HasError<ValidationError>())
        {
            return (400, "Validation Error");
        }

        if (result.HasError<UnauthorizedError>())
        {
            return (403, "You dont have permission to access this resource.");
        }

        if (result.HasError<NotFoundError>())
        {
            return (404, "Resource Not Found.");
        }

        if (result.HasError<ConflictError>())
        {
            return (409, "A resource with the same content already exists.");
        }

        return (500, "Internal Server Error");
    }
}
