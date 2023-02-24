namespace CarRentalSystem.Application;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

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
