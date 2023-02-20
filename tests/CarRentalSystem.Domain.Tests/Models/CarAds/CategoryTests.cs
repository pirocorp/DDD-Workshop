namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using System;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.CarAds;

using FluentAssertions;
using Xunit;

public class CategoryTests
{
    [Fact]
    public void ValidCategoryShouldNotThrowException()
    {
        // Arrange
        var name = "Valid name";
        var description = "Valid description text";

        // Act
        var act = () => new Category(name, description);

        // Assert
        act.Should().NotThrow<InvalidCarAdException>();
        act().Name.Should().Be(name);
        act().Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void InvalidNameShouldThrowException(string name)
    {
        // Act
        Action act = () => new Category(name, "Valid description text");

        // Assert
        act.Should().Throw<InvalidCarAdException>();
    }
}
