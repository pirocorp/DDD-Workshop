namespace CarRentalSystem.Domain.Tests.Models.Dealers;

using CarRentalSystem.Domain.Models.Dealers;
using FluentAssertions;
using Xunit;

public class PhoneNumberTests
{
    [Fact]
    public void PhoneNumberIsCreatedCorrectlyWithValidNumber()
    {
        // Arrange
        var number = "+359123456789";

        // Act
        var act = () => new PhoneNumber(number);

        // Assert
        act.Should().NotThrow();
        act().Number.Equals(number).Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversionFromStringWorksCorrectly()
    {
        // Arrange
        var phoneString = "+359123456789";

        // Act
        PhoneNumber phone = phoneString;

        // Assert
        phone.Number.Should().Be(phoneString);
    }

    [Fact]
    public void ImplicitConversionToStringWorksCorrectly()
    {
        // Arrange
        var phoneString = "+359123456789";
        var phoneNumber = new PhoneNumber(phoneString);

        // Act
        string result = phoneNumber;

        // Assert
        result.Should().Be(phoneNumber);
    }
}
