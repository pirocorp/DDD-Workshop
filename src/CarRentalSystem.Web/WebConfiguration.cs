namespace CarRentalSystem.Web;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Web.Services;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public static class WebConfiguration
{
    public static IServiceCollection AddWebServices(
        this IServiceCollection services)
    {
        services
            .AddScoped<ICurrentUser, CurrentUserService>()
            .AddValidatorsFromAssemblyContaining<Result>()
            .AddFluentValidationAutoValidation()
            .AddControllers();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }
}
