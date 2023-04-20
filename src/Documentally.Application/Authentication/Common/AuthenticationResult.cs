using Documentally.Domain.Entities;

namespace Documentally.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token
);
