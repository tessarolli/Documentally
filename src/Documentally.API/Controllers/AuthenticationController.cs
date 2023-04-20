using Documentally.API.Common.Controllers;
using Documentally.Application.Authentication.Commands.Register;
using Documentally.Application.Authentication.Common;
using Documentally.Application.Authentication.Queries.Login;
using Documentally.Contracts.Authentication;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

[ApiController]
[Route("authentication")]
public class AuthenticationController : ResultControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(request);
        var authenticationResult = await _mediator.Send(registerCommand);

        

        return ValidateResult(authenticationResult,
            () => Ok(_mapper.Map<AuthenticationResponse>(authenticationResult.Value)),
            () => Problem()
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        var loginQuery = _mapper.Map<LoginQuery>(request);
        var authenticationResult = await _mediator.Send(loginQuery);

        return ValidateResult(authenticationResult,
            () => Ok(_mapper.Map<AuthenticationResponse>(authenticationResult.Value)),
            () => Problem()
        );
    }
}
