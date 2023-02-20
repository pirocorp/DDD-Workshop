namespace CarRentalSystem.Domain.Tests.Common;

using CarRentalSystem.Domain.Exceptions;

using FluentAssertions;
using Xunit;

using static CarRentalSystem.Domain.Common.Guard;

public class GuardTests
{
    private class TestException : BaseDomainException
    { }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AgainstEmptyStringGuardThrowsIfStringIsNullOrEmpty(string message)
    {
        // Act
        var act = () => AgainstEmptyString<TestException>(message);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void AgainstEmptyStringGuardNotThrowsWithString()
    {
        // Act
        var act = () => AgainstEmptyString<TestException>("Not Empty String");

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("Too Short String", 60, 150)]
    [InlineData("Too Long String", 5, 10)]
    public void ForStringLengthThrowsIfStringIsNotInGivenRange(string message, int min, int max)
    {
        // Act
        var act = () => ForStringLength<TestException>(message, min, max);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void ForStringLengthNotThrowsWithStringInGivenRange()
    {
        // Act
        var act = () => ForStringLength<TestException>("Valid String", 5, 60);

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(50, 5, 40)]
    [InlineData(5, 6, 40)]
    [InlineData(41, 6, 40)]
    public void AgainstOutOfRangeThrowsIfValueIsOutOfRange(int number, int min, int max)
    {
        // Act
        var act = () => AgainstOutOfRange<TestException>(number, min, max);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void AgainstOutOfRangeNotThrowsIfValueIsInRange()
    {
        // Act
        var act = () => AgainstOutOfRange<TestException>(10, 5, 20);

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(50.5, 5.5, 40.5)]
    [InlineData(5.999, 6, 40)]
    [InlineData(40.00001, 6, 40)]
    public void AgainstOutOfRangeThrowsIfValueIsOutOfRangeForDecimals(decimal number, decimal min, decimal max)
    {
        // Act
        var act = () => AgainstOutOfRange<TestException>(number, min, max);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void AgainstOutOfRangeNotThrowsIfValueIsInRangeForDecimals()
    {
        // Act
        var act = () => AgainstOutOfRange<TestException>(0.30001M, 0.3M, 20M);

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("http://")]
    [InlineData("asd")]
    [InlineData("sdfsdfsdfsdf.asdfasdfq451234592134u$##z.com")]
    public void ForValidUrlShouldThrowIfUrlIsInvalid(string url)
    {
        // Act
        var act = () => ForValidUrl<TestException>(url);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void ForValidUrlShouldThrowIfUrlIsMoreThenMaxAllowedLength()
    {
        var url = $"http://{new string('a', 2048)}.com";

        // Act
        var act = () => ForValidUrl<TestException>(url);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void ForValidUrlShouldNotThrowIfUrlIsValid()
    {
        // Act
        var act = () => ForValidUrl<TestException>("http://google.com");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateRegexThrowsIfRegexDoNotMatch()
    {
        // Arrange
        var pattern = "\\+[0-9]*";

        // Act
        var act = () => ValidateRegex("Invalid string", pattern, string.Empty);

        // Assert
        act.Should().Throw<InvalidPhoneNumberException>();
    }

    [Fact]
    public void ValidateRegexNotThrowsIfRegexMatch()
    {
        // Arrange
        var pattern = "\\+[0-9]*";

        // Act
        var act = () => ValidateRegex("+359123456789", pattern, string.Empty);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void AgainstShouldThrowWithSameValues()
    {
        // Arrange
        var a = "abc";
        var b = "abc";

        // Act
        var act = () => Against<TestException>(a, b);

        // Assert
        act.Should().Throw<TestException>();
    }

    [Fact]
    public void AgainstShouldNotThrowWithDifferentValues()
    {
        // Arrange
        var a = "abc";
        var b = "abd";

        // Act
        var act = () => Against<TestException>(a, b);

        // Assert
        act.Should().NotThrow();
    }
}
