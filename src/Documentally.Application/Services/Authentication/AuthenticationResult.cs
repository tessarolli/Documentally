using Documentally.Domain.Entities;

namespace Documentally.Application.Services.Authentication;

public record AuthenticationResult(
    User User,
    string Token
);


