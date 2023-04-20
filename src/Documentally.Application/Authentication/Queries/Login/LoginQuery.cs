using Documentally.Application.Authentication.Common;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<Result<AuthenticationResult>>;
