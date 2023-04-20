using Documentally.API.DependencyInjection;
using Documentally.Application.DependencyInjection;
using Documentally.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}


var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");

    app.Map("/error", (HttpContext context) => 
    {
        Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Results.Problem(exception?.Message);

    });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}