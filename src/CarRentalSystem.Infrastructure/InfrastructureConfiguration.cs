namespace CarRentalSystem.Infrastructure;

using System;
using System.IO;
using System.Text;

using CarRentalSystem.Application;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Application.Features.Identity;
using CarRentalSystem.Infrastructure.Identity;
using CarRentalSystem.Infrastructure.Persistence;
using CarRentalSystem.Web.Features;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration? configuration)
            => services
                .AddDatabase(configuration)
                .AddRepositories()
                .AddIdentity(configuration)
                .AddSwagger();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IRepository<>)))
                .AsMatchingInterface()
                .WithTransientLifetime());

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration? configuration)
        => services
            .AddDbContext<CarRentalDbContext>(options => options.UseSqlServer(
                configuration?.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CarRentalDbContext).Assembly.FullName)))
            .AddTransient<IInitializer, CarRentalDbInitializer>();

    private static IServiceCollection AddIdentity(
        this IServiceCollection services,
        IConfiguration? configuration)
    {
        services
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<CarRentalDbContext>();

        var secret = configuration
            ?.GetSection(nameof(ApplicationSettings))
            .GetValue<string>(nameof(ApplicationSettings.Secret));

        if (secret is null)
        {
            throw new InvalidOperationException($"Missing {nameof(ApplicationSettings)}");
        }

        var key = Encoding.ASCII.GetBytes(secret);

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddTransient<IIdentity, IdentityService>();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGeneratorService>();

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

            var xmlFilename = $"{typeof(IController).Assembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
}
