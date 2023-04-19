using Documentally.Application.Services.Authentication;
using Documentally.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

[ApiController]
[Route("authentication")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var authenticationResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        var authenticationResponse = new AuthenticationResponse(
            authenticationResult.User.Id.Value ,
            authenticationResult.User.FirstName,
            authenticationResult.User.LastName,
            authenticationResult.User.Email,
            authenticationResult.Token
        );

        return Ok(authenticationResponse);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authenticationResult = _authenticationService.Login(request.Email, request.Password);

        var authenticationResponse = new AuthenticationResponse(
            authenticationResult.User.Id.Value,
            authenticationResult.User.FirstName,
            authenticationResult.User.LastName,
            authenticationResult.User.Email,
            authenticationResult.Token
        );
        return Ok(authenticationResponse);

    }
}
