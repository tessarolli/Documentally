﻿using Documentally.API.Common.Mappings;

namespace Documentally.API.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();

        return services;
    }
}
