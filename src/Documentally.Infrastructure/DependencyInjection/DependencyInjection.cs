using System.Text;
using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Application.Interfaces.Persistence;
using Documentally.Infrastructure.Authentication;
using Documentally.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Documentally.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(configuration)
            .AddPersistance();


        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {

        services.AddScoped<IUserRepository, UserRepository>();


        //services.AddDbContext<BuberDinnerDbContext>(options =>
        //    options.UseSqlServer("Server=localhost;Database=BuberDinner;User Id=sa;Password=amiko123!;TrustServerCertificate=True"));

        //services.AddScoped<IMenuRepository, MenuRepository>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        //services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateLifetime = true,
        //        ValidateIssuer = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = jwtSettings.Issuer,
        //        ValidAudience = jwtSettings.Audience,
        //        ValidateAudience = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        //    });

        return services;
    }
}
