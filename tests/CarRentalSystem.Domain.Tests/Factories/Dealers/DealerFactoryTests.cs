namespace CarRentalSystem.Domain.Tests.Factories.Dealers;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Factories.Dealers;
using FluentAssertions;
using Xunit;

public class DealerFactoryTests
{
    [Fact]
    public void BuildShouldThrowIfNameIsNotSet()
    {
        // Assert
        var carAdFactory = new DealerFactory();

        // Act
        var act = () => carAdFactory
            .WithPhoneNumber("+1234567890")
            .Build();

        // Assert
        act.Should().Throw<InvalidDealerException>();
    }

    [Fact]
    public void BuildShouldThrowIfPhoneIsNotSet()
    {
        // Assert
        var carAdFactory = new DealerFactory();

        // Act
        var act = () => carAdFactory
            .WithName("Test Dealer")
            .Build();

        // Assert
        act.Should().Throw<InvalidPhoneNumberException>();
    }

    [Fact]
    public void BuildShouldCreateDealerIfEveryPropertyIsSet()
    {
        // Assert
        var carAdFactory = new DealerFactory();

        // Act
        var carAd = carAdFactory
            .WithName("Test Dealer")
            .WithPhoneNumber("+1234567890")
            .Build();

        // Assert
        carAd.Should().NotBeNull();
    }

    [Fact]
    public void BuildShouldCreateDealerWithParameters()
    {
        // Assert
        var carAdFactory = new DealerFactory();

        // Act
        var carAd = carAdFactory
            .Build("Test Dealer", "+1234567890");

        // Assert
        carAd.Should().NotBeNull();
    }
}
