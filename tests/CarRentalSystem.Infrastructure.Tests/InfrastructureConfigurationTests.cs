namespace CarRentalSystem.Infrastructure.Tests;

using System.Reflection;
using CarRentalSystem.Application.Features.CarAds;
using CarRentalSystem.Infrastructure.Persistence;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class InfrastructureConfigurationTests
{
    [Fact]
    public void AddRepositoriesShouldRegisterRepositories()
    {
        // Arrange
        var serviceCollection = new ServiceCollection()
            .AddDbContext<CarRentalDbContext>(
                opts 
                    => opts.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        var method = typeof(InfrastructureConfiguration)
            .GetMethod("AddRepositories", BindingFlags.Static | BindingFlags.NonPublic);

        var parameters = new object[] { serviceCollection };

        // Act
        var services = ((IServiceCollection?) method
                ?.Invoke(serviceCollection, parameters) 
                ?? throw new InvalidOperationException($"AddRepositories method in {nameof(InfrastructureConfiguration)} not found"))
            .BuildServiceProvider();

        // Assert
        services
            .GetService<ICarAdRepository>()
            .Should()
            .NotBeNull();
    }
}
