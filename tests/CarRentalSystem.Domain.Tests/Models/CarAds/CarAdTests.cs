namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using System;

using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;
using FluentAssertions;
using Xunit;

public class CarAdTests
{
    [Fact]
    public void CarAdCreationShouldWorksCorrectly()
    {
        // Arrange
        var dummy = new
        {
            Id = Guid.NewGuid(),
            Manufacturer = new Manufacturer("Valid manufacturer"),
            Model = "Valid model",
            Category = A.Dummy<Category>(),
            ImageUrl = "https://piro.com",
            PricePerDay = 10,
            Options = new Options(true, 4, TransmissionType.Automatic),
            IsAvailable = true
        };

        // Act
        var actual = new CarAd(
            dummy.Manufacturer,
            dummy.Model,
            dummy.Category,
            dummy.ImageUrl,
            dummy.PricePerDay,
            dummy.Options,
            dummy.IsAvailable);

        // Assert
        actual.Should().BeEquivalentTo(
            dummy,
            opt => opt
                .ComparingByMembers(typeof(CarAd))
                .Excluding(x => x.Id));
    }

    [Fact]
    public void ChangeAvailabilityShouldMutateIsAvailable()
    {
        // Arrange
        var carAd = A.Dummy<CarAd>();

        // Act
        carAd.ChangeAvailability();

        // Assert
        carAd.IsAvailable.Should().BeFalse();
    }
}
