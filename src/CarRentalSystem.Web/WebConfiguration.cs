namespace CarRentalSystem.Web;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Web.Services;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}
