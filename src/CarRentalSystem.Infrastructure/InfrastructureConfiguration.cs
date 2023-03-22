namespace CarRentalSystem.Infrastructure;

using System;
using System.Text;

using CarRentalSystem.Application;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Application.Features.Identity;
using CarRentalSystem.Infrastructure.Identity;
using CarRentalSystem.Infrastructure.Persistence;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration? configuration)
            => services
                .AddDatabase(configuration)
                .AddRepositories()
                .AddIdentity(configuration);

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration? configuration)
        => services
            .AddDbContext<CarRentalDbContext>(
                options => options.UseSqlServer(configuration?.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(CarRentalDbContext).Assembly.FullName)))
            .AddTransient<IInitializer, CarRentalDbInitializer>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                .AsMatchingInterface()
                .WithTransientLifetime());

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
}
