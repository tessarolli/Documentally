using Documentally.Application.Interfaces.Persistence;
using Documentally.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Documentally.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
