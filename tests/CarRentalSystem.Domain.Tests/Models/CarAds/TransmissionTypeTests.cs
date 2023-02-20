namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using CarRentalSystem.Domain.Models.CarAds;

using FluentAssertions;
using Xunit;

public class TransmissionTypeTests
{
    [Fact]
    public void AllTransmissionTypesAreDeclared()
    {
        // Assert
        TransmissionType.Automatic.Name.Should().Be(nameof(TransmissionType.Automatic));
        TransmissionType.Automatic.Value.Should().Be(2);

        TransmissionType.Manual.Name.Should().Be(nameof(TransmissionType.Manual));
        TransmissionType.Manual.Value.Should().Be(1);
    }
}
