// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Mappings;

namespace Documentally.API;

/// <summary>
/// Dependency Injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add Presentation's layers dependencies.
    /// </summary>
    /// <param name="services">IServiceCollection for the dependencies to be injected at.</param>
    /// <returns>IServiceCollection with dependencies injected.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
