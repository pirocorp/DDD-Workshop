namespace CarRentalSystem.Web;

using System;
using System.IO;
using System.Reflection;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Web.Services;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class WebConfiguration
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services)
    {
        // Async Validation Are no Longer Supported in version 11 https://github.com/FluentValidation/FluentValidation/issues/1959
        // https://github.com/FluentValidation/FluentValidation/blob/7f8a6cbc44fbf1d1bb9110ad093c2673ebdd4c0f/docs/aspnet.md
        // Auto validation is not asynchronous: If your validator contains asynchronous rules
        // then your validator will not be able to run. You will receive an exception at runtime
        // if you attempt to use an asynchronous validator with auto-validation.
        // So validation is handled entirely from MediatR

        services
            .AddScoped<ICurrentUser, CurrentUserService>()
            .AddValidatorsFromAssemblyContaining<Result>()
            .AddControllers();

        services
            .AddSwagger();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Car Rental System API",
                Description = "An ASP.NET API designed with DDD in mind",
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://mit-license.org/")
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.EnableAnnotations();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
}
