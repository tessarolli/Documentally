// <copyright file="AuthenticationController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Authentication.Commands.Register;
using Documentally.Application.Authentication.Queries.Login;
using Documentally.Contracts.Authentication;
using Documentally.Domain.Enums;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Authentication Controller.
/// </summary>
[Route("authentication")]
public class AuthenticationController : ResultControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    public AuthenticationController(IMediator mediator, IMapper mapper, ILogger<AuthenticationController> logger)
        : base(logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    /// <summary>
    /// Endpoint to register an user.
    /// </summary>
    /// <param name="request">User data for registration.</param>
    /// <returns>The result of the register operation.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        logger.LogInformation("POST /Register called");

        var registerCommand = mapper.Map<RegisterCommand>(request);
        var authenticationResult = await mediator.Send(registerCommand);

        return ValidateResult(
            authenticationResult,
            () => Ok(mapper.Map<AuthenticationResponse>(authenticationResult.Value)),
            () => Problem());
    }

    /// <summary>
    /// Endpoint for a user to perform Login.
    /// </summary>
    /// <param name="request">User data for login.</param>
    /// <returns>The result of the login operation.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        logger.LogInformation("POST /Login called");

        var loginQuery = mapper.Map<LoginQuery>(request);
        var authenticationResult = await mediator.Send(loginQuery);

        return ValidateResult(
            authenticationResult,
            () => Ok(mapper.Map<AuthenticationResponse>(authenticationResult.Value)),
            () => Problem());
    }
}
