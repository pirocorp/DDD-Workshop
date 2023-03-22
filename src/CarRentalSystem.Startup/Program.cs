namespace CarRentalSystem.Startup;

using System;
using System.Collections.Generic;
using CarRentalSystem.Application;
using CarRentalSystem.Domain;
using CarRentalSystem.Infrastructure;
using CarRentalSystem.Web;
using CarRentalSystem.Web.Middleware.ValidationExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static IConfiguration? configuration;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureConfiguration(builder.Configuration);
        ConfigureServices(builder.Services);

        var app = builder.Build();

        ConfigureMiddleware(app);
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureConfiguration(IConfiguration config)
    {
        configuration = config;
    }

    private static void ConfigureServices(IServiceCollection services)
        => services
            .AddDomainServices()
            .AddApplicationServices(configuration)
            .AddInfrastructureServices(configuration)
            .AddWebServices();

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseValidationExceptionHandler();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Initialize();
    }

    private static void ConfigureEndpoints(IEndpointRouteBuilder app)
    {
        app.MapControllers();
    }
}
