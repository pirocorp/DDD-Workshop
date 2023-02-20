namespace CarRentalSystem.Domain.Tests.Common;

using System;
using System.Linq;
using CarRentalSystem.Domain.Common;
using FluentAssertions;
using Xunit;

public class EnumerationTests
{
    private class TestEnumeration : Enumeration
    {
        public static readonly TestEnumeration One = new (1, nameof(One));
        public static readonly TestEnumeration Two = new (2, nameof(Two));

        private TestEnumeration(int value, string name) 
            : base(value, name)
        { }
    }

    [Fact]
    public void EnumerationIsCreatedCorrectly()
    {
        // Assert
        TestEnumeration.One.Name.Should().Be(nameof(TestEnumeration.One));
        TestEnumeration.One.Value.Should().Be(1);

        TestEnumeration.Two.Name.Should().Be(nameof(TestEnumeration.Two));
        TestEnumeration.Two.Value.Should().Be(2);
    }

    [Fact]
    public void EnumerationToStringReturnsNameOfTheEnumeration()
    {
        // Arrange
        var first = TestEnumeration.One;
        var second = TestEnumeration.Two;

        // Act
        var firstString = first.ToString();
        var secondString = second.ToString();

        // Assert
        firstString.Should().Be(first.Name);
        secondString.Should().Be(second.Name);
    }

    [Fact]
    public void EnumerationGetAllMethodReturnsListWithEnumerationValues()
    {
        // Act
        var enums = Enumeration
            .GetAll<TestEnumeration>()
            .ToArray();

        // Assert
        enums.Should().HaveCount(2);
        enums.Should().Contain(TestEnumeration.One);
        enums.Should().Contain(TestEnumeration.Two);
    }

    [Fact]
    public void ParseEnumerationFromValue()
    {
        // Act
        var testEnum = Enumeration.FromValue<TestEnumeration>(1);

        // Assert
        testEnum.Should().BeEquivalentTo(TestEnumeration.One);
    }

    [Fact]
    public void ParseEnumerationFromString()
    {
        // Act
        var testEnum = Enumeration.FromName<TestEnumeration>(nameof(TestEnumeration.Two));

        // Assert
        testEnum.Should().BeEquivalentTo(TestEnumeration.Two);
    }

    [Fact]
    public void ParseEnumerationThrowsWithInvalidEnumeration()
    {
        // Act
        var act1 = () => Enumeration.FromValue<TestEnumeration>(-5);
        var act2 = () => Enumeration.FromName<TestEnumeration>("Invalid Name");

        // Assert
        act1.Should().Throw<InvalidOperationException>();
        act2.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void EnumerationEqualsReturnsFalseComparedToNonEnumeration()
    {
        // Act
        var result = TestEnumeration.One.Equals(new object());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EnumerationEqualsReturnsTrueForSameEnumeration()
    {
        // Arrange
        var testEnum = Enumeration.FromName<TestEnumeration>(nameof(TestEnumeration.One));

        // Act
        var result = testEnum.Equals(TestEnumeration.One);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EnumerationHashCodeIsDifferentFromTypeHashCodeAndValueHashCode()
    {
        // Arrange
        var enumeration = TestEnumeration.One;
        var typeHashCode = enumeration.GetType().GetHashCode();
        var valueHashCode = enumeration.Value.GetHashCode();
        var entityHashCode = enumeration.GetHashCode();

        // Act
        var result1 = entityHashCode != typeHashCode;
        var result2 = entityHashCode != valueHashCode;

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    [Fact]
    public void EnumerationCompareToIsDoneBasedOnValue()
    {
        // Act
        var result = TestEnumeration.One.CompareTo(TestEnumeration.Two)
            == TestEnumeration.One.Value.CompareTo(TestEnumeration.Two.Value);

        // Assert
        result.Should().BeTrue();
    }
}
