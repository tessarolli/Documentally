// <copyright file="AuthenticationController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Authentication.Commands.Register;
using Documentally.Application.Authentication.Queries.Login;
using Documentally.Application.Authentication.Results;
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
public class AuthenticationController : ResultControllerBase<AuthenticationController>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    /// <param name="exceptionHandlingService">Injected exceptionHandlingService.</param>
    public AuthenticationController(IMediator mediator, IMapper mapper, ILogger<AuthenticationController> logger, IExceptionHandlingService exceptionHandlingService)
        : base(mediator, mapper, logger, exceptionHandlingService)
    {
    }

    /// <summary>
    /// Endpoint to register an user.
    /// </summary>
    /// <param name="request">User data for registration.</param>
    /// <returns>The result of the register operation.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request) =>
         await HandleRequestAsync<RegisterCommand, AuthenticationResult, AuthenticationResponse>(request);

    /// <summary>
    /// Endpoint for a user to perform Login.
    /// </summary>
    /// <param name="request">User data for login.</param>
    /// <returns>The result of the login operation.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginRequest request) =>
       await HandleRequestAsync<LoginQuery, AuthenticationResult, AuthenticationResponse>(request);
}
