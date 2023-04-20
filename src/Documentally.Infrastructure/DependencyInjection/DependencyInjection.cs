// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Application.Interfaces.Persistence;
using Documentally.Infrastructure.Authentication;
using Documentally.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Documentally.Infrastructure.DependencyInjection;

/// <summary>
/// Dependency Injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add Infrastructure dependencies.
    /// </summary>
    /// <param name="services">Injected services.</param>
    /// <param name="configuration">Injected configuration.</param>
    /// <returns>Services with dependencies injected.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(configuration)
            .AddPersistance();

        return services;
    }

    /// <summary>
    /// Add Persistence dependencies.
    /// </summary>
    /// <param name="services">Injected services.</param>
    /// <returns>Services with dependencies injected.</returns>
    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        /*
        //services.AddDbContext<BuberDinnerDbContext>(options =>
        //    options.UseSqlServer("Server=localhost;Database=BuberDinner;User Id=sa;Password=amiko123!;TrustServerCertificate=True"));

        //services.AddScoped<IMenuRepository, MenuRepository>();
        */
        return services;
    }

    /// <summary>
    /// Add Authentication dependencies.
    /// </summary>
    /// <param name="services">Injected services.</param>
    /// <param name="configuration">Injected configuration.</param>
    /// <returns>Services with dependencies injected.</returns>
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
