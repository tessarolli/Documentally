// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Documentally.API.Common.Mappings;

/// <summary>
/// Dependency Injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add dependencies for mapster type mapping.
    /// </summary>
    /// <param name="services">Injected services.</param>
    /// <returns>IServiceCollection with dependencies injected.</returns>
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
