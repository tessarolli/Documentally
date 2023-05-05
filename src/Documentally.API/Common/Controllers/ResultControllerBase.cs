// <copyright file="ResultControllerBase.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Azure.Core;
using Documentally.API.Controllers;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Authentication.Commands.Register;
using Documentally.Application.Common.Errors;
using Documentally.Contracts.Authentication;
using Documentally.Domain.Common;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Common.Controllers;

/// <summary>
/// Base Class for Api Controller that Handles Validation Results.
/// </summary>
/// <typeparam name="TController">Type.</typeparam>
[ApiController]
[Authorize]
public class ResultControllerBase<TController> : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly IExceptionHandlingService exceptionHandlingService;

    /// <summary>
    /// ILogger instance.
    /// </summary>
#pragma warning disable SA1401 // Fields should be private
    protected readonly ILogger logger;
#pragma warning restore SA1401 // Fields should be private

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultControllerBase{T}"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">ILogger injected.</param>
    /// <param name="exceptionHandlingService">IExceptionHandlingService injected.</param>
    public ResultControllerBase(IMediator mediator, IMapper mapper, ILogger<TController> logger, IExceptionHandlingService exceptionHandlingService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
        this.exceptionHandlingService = exceptionHandlingService;
    }

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
            logger.LogInformation("Request Success");

            return success.Invoke();
        }

        failure.Invoke();

        var errorMessages = result.Errors.Select(e => e.Message).ToList();

        logger.LogInformation("Request Failure with message(s):\n{errorMessages}", errorMessages);

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
    /// Handler for received requests.
    /// </summary>
    /// <typeparam name="Tin">The Type of the Command or Query to execute.</typeparam>
    /// <typeparam name="Tresult">The Type of the Command or Query Result.</typeparam>
    /// <typeparam name="Tout">The Type of the Response (contract).</typeparam>
    /// <param name="request">The input received in the request.</param>
    /// <returns>An ActionResult for sending to the client.</returns>
    [NonAction]
    public async Task<IActionResult> HandleRequestAsync<Tin, Tresult, Tout>(object? request = null)
    {
        Result<Tresult> result;
        Tout? response = default;

        try
        {
            object command;
            if (request is null)
            {
                command = Activator.CreateInstance(typeof(Tin))!;
            }
            else
            {
                if (request is long idRequest)
                {
                    command = Activator.CreateInstance(typeof(Tin), idRequest)!;
                }
                else
                {
                    command = mapper.Map<Tin>(request)!;
                }
            }

            if (typeof(Tresult) == typeof(Result))
            {
                result = await mediator.Send((IRequest<Result>)command);
            }
            else
            {
                result = await mediator.Send((IRequest<Result<Tresult>>)command);
            }

            if (result.IsSuccess)
            {
                response = mapper.Map<Tout>(result.Value!);
            }
        }
        catch (Exception ex)
        {
            result = exceptionHandlingService.HandleException(ex, logger);
        }

        return ValidateResult(
                   result,
                   () => Ok(response),
                   () => Problem());
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
