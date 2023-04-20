using Documentally.Application.Authentication.Common;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<Result<AuthenticationResult>>;
