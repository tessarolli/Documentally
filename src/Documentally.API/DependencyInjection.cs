// <copyright file="DependencyInjection.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Mappings;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
        services.AddHttpContextAccessor();

        services.AddMappings();

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
         {
             options.SwaggerDoc("v1", new OpenApiInfo
             {
                 Version = "v1",
                 Title = "Documentally API",
                 Description = "Documentally is a monolithic application built for scaling using .NET Core and PostgreSQL as the primary data storage. \r\nIt follows Clean Architecture, Domain-Driven Design (DDD) and CQRS (Commands and Queries Responsibility Seggregation) principles to ensure the code is organized, maintainable, and scalable.\r\nIts loose coupling design makes it easy to refactor into a microservices architecture with minimal changes. \r\nThe application provides an API for users to upload and download documents with metadata such as posted date, name, description, and category, as well as manage user groups and access permissions.",
             });

             var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
             options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

             options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
             {
                 Type = SecuritySchemeType.Http,
                 In = ParameterLocation.Header,
                 Name = HeaderNames.Authorization,
                 Scheme = "Bearer",
             });
             options.OperationFilter<SecureEndpointAuthRequirementFilter>();
         });

        return services;
    }
}
