// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Documentally.Application.DependencyInjection;

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

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }
}
