// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Text;
using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Common.Abstractions;
using Documentally.Infrastructure;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.Authentication;
using Documentally.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Documentally.Infrastructure;

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
            .AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddDebug();
            })
            .AddPersistance()
            .AddAuthentication(configuration);

        services.AddScoped<IPasswordHasher, PasswordHasher>();

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
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddScoped<IPostgresSqlConnectionFactory, PostgresSqlConnectionFactory>();

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
        var jwtSettings = new JwtSettings();

        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });
        return services;
    }
}
