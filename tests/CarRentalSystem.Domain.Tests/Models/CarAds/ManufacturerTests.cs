namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.CarAds;

using FluentAssertions;
using Xunit;

public class ManufacturerTests
{
    [Fact]
    public void ManufacturerIsCreatedCorrectly()
    {
        // Arrange
        var name = "Valid Manufacturer";

        // Act
        var manufacturer = new Manufacturer(name);

        // Assert
        manufacturer.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("I")]
    [InlineData("Global Invalid Manufacturer")]
    public void ManufacturerThrowsWithInvalidName(string name)
    {
        // Act
        var act = () =>  new Manufacturer(name);

        // Assert
        act.Should().Throw<InvalidCarAdException>();
    }
}
