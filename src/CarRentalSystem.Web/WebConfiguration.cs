namespace CarRentalSystem.Web;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Web.Services;

using Microsoft.Extensions.DependencyInjection;

public static class WebConfiguration
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services)
    {
        services.AddControllers();

        services.AddTransient<ICurrentUser, CurrentUserService>();

        return services;
    }
}
