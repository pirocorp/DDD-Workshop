namespace CarRentalSystem.Application;

using System;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration? configuration)
        => services
            .Configure<ApplicationSettings>(
                configuration?.GetSection(nameof(ApplicationSettings))
                    ?? throw new InvalidOperationException($"Missing {nameof(ApplicationSettings)}"),
                options => options.BindNonPublicProperties = true)
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
}
