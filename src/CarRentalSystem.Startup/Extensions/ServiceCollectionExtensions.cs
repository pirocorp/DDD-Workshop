namespace CarRentalSystem.Startup.Extensions;

using System;
using System.IO;
using System.Reflection;
using CarRentalSystem.Web.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Car Rental System API",
                Description = "An ASP.NET API designed with DDD in mind",
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://mit-license.org/")
                }
            });

            var xmlFilename = $"{typeof(IController).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}
