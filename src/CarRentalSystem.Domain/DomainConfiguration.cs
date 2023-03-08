namespace CarRentalSystem.Domain;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Factories;
using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.Extensions.DependencyInjection;

public static class DomainConfiguration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IFactory<>)))
                .AsMatchingInterface()
                .WithTransientLifetime())
            .AddTransient<IInitialData, CategoryData>();
}
