namespace CarRentalSystem.Domain.Tests;

using CarRentalSystem.Domain.Factories.CarAds;
using CarRentalSystem.Domain.Factories.Dealers;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class DomainConfigurationTests
{
    [Fact]
    public void AddDomainServicesShouldRegisterFactories()
    {
        // Arrange
        var serviceCollections = new ServiceCollection();

        // Act
        var services = serviceCollections
            .AddDomainServices()
            .BuildServiceProvider();

        // Assert
        services
            .GetService<IDealerFactory>()
            .Should()
            .NotBeNull();

        services
            .GetService<ICarAdFactory>()
            .Should()
            .NotBeNull();
    }
}
