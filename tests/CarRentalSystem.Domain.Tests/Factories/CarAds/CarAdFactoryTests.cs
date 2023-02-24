namespace CarRentalSystem.Domain.Tests.Factories.CarAds;

using System;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Factories.CarAds;
using CarRentalSystem.Domain.Models.CarAds;

using FluentAssertions;
using Xunit;

public class CarAdFactoryTests
{
    [Fact]
    public void BuildShouldThrowExceptionIfManufacturerIsNotSet()
    {
        // Assert
        var carAdFactory = new CarAdFactory();

        // Act
        var act = () => carAdFactory
            .WithCategory("TestCategory", "Valid Test Description")
            .WithOptions(true, 2, TransmissionType.Automatic)
            .Build();

        // Assert
        act.Should().Throw<InvalidCarAdException>();
    }

    [Fact]
    public void BuildShouldThrowExceptionIfCategoryIsNotSet()
    {
        // Assert
        var carAdFactory = new CarAdFactory();

        // Act
        Action act = () => carAdFactory
            .WithManufacturer("TestManufacturer")
            .WithOptions(true, 2, TransmissionType.Automatic)
            .Build();

        // Assert
        act.Should().Throw<InvalidCarAdException>();
    }

    [Fact]
    public void BuildShouldThrowExceptionIfOptionsAreNotSet()
    {
        // Assert
        var carAdFactory = new CarAdFactory();

        // Act
        Action act = () => carAdFactory
            .WithManufacturer("TestManufacturer")
            .WithCategory("TestCategory", "Valid Test Description")
            .Build();

        // Assert
        act.Should().Throw<InvalidCarAdException>();
    }

    [Fact]
    public void BuildShouldCreateCarAdIfEveryPropertyIsSet()
    {
        // Assert
        var carAdFactory = new CarAdFactory();

        // Act
        var carAd = carAdFactory
            .WithManufacturer("TestManufacturer")
            .WithCategory("TestCategory", "Valid Test Description")
            .WithOptions(true, 2, TransmissionType.Automatic)
            .WithImageUrl("http://test.image.url")
            .WithModel("TestModel")
            .WithPricePerDay(10)
            .Build();

        // Assert
        carAd.Should().NotBeNull();
    }
}
