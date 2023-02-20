namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.CarAds;

using FluentAssertions;
using Xunit;

public class OptionsTests
{
    [Fact]
    public void OptionsAreCreatedCorrectly()
    {
        // Arrange
        var hasClimateControl = true;
        var numberOfSeats = 4;
        var transmissionType = TransmissionType.Automatic;

        // Act
        var options = new Options(hasClimateControl, numberOfSeats, transmissionType);

        // Assert
        options.HasClimateControl.Should().Be(hasClimateControl);
        options.NumberOfSeats.Should().Be(numberOfSeats);
        options.TransmissionType.Should().Be(transmissionType);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(51)]
    public void OptionsThrowsIfCreatedWithInvalidNumberOfSeats(int numberOfSeats)
    {
        // Arrange
        var hasClimateControl = true;
        var transmissionType = TransmissionType.Automatic;

        // Act
        var act = () => new Options(hasClimateControl, numberOfSeats, transmissionType);

        // Assert
        act.Should().Throw<InvalidOptionsException>();
    }
}
