namespace CarRentalSystem.Domain.Tests.Models.Dealers;

using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Models.Dealers;
using FakeItEasy;
using FluentAssertions;
using Xunit;

public class DealerTests
{
    [Fact]
    public void DealerIsCreatedCorrectly()
    {
        // Arrange
        var name = "Piro";
        var phone = "+359123456789";

        // Act
        var dealer = new Dealer(name, phone);

        // Assert
        dealer.Name.Should().Be(name);
        dealer.PhoneNumber.Number.Should().Be(phone);
        dealer.CarAds.Should().NotBeNull();
    }

    [Fact]
    public void DealerAddCarAdShouldAdCarAdToDealerAdsCollection()
    {
        // Arrange
        var dealer = A.Dummy<Dealer>();
        var carAd = A.Dummy<CarAd>();

        // Act
        dealer.AddCarAd(carAd);

        // Assert
        dealer.CarAds.Count.Should().Be(1);
    }
}
