// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application;
using Documentally.Application.Common.Behaviors;
using Documentally.Application.EventBus;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Documentally.Application;

/// <summary>
/// Provides support for Dependency Injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Injects all dependency for the Application Layer.
    /// </summary>
    /// <param name="services">IServiceCollection instance.</param>
    /// <returns>IServiceCollection with dependencies injected.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(assembly);

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }

    /// <summary>
    /// Injects all dependency for the MediatR.
    /// </summary>
    /// <param name="services">IServiceCollection instance.</param>
    /// <param name="assembly">Assembly.</param>
    /// <returns>IServiceCollection with dependencies injected.</returns>
    public static IServiceCollection AddMediatR(this IServiceCollection services, System.Reflection.Assembly assembly)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineRequestValidationBehavior<,>));

        services.AddSingleton<IDomainEventBus, DomainEventBus>();

        return services;
    }
}
