namespace CarRentalSystem.Domain.Tests.Common;

using CarRentalSystem.Domain.Common;

using FluentAssertions;
using Xunit;

public class ValueObjectTests
{
    private class ValueObjectTest : ValueObject
    {
        public ValueObjectTest()
        {
            this.FirstValue = string.Empty;
        }

        public string? FirstValue { get; set; }

        public int SecondValue { get; set; }
    }

    private class ValueObjectTest2 : ValueObject
    { }

    [Fact]
    public void ValueObjectIsCreatedCorrectly()
    {
        // Act
        var value = new ValueObjectTest();

        // Assert
        value.Should().NotBeNull();
    }

    [Fact]
    public void ValueObjectHashCodeIsDifferentFromHashCodeOfItsFields()
    {
        // Arrange
        var valueObject = new ValueObjectTest
        {
            FirstValue = "First Value",
            SecondValue = 42
        };

        // Act
        var firstValueHashCode = valueObject.FirstValue.GetHashCode();
        var secondValueHashCode = valueObject.SecondValue.GetHashCode();
        var valueObjectHashCode = valueObject.GetHashCode();

        // Assert
        valueObjectHashCode.Should().NotBe(firstValueHashCode);
        valueObjectHashCode.Should().NotBe(secondValueHashCode);
    }

    [Fact]
    public void ValueObjectHashCodeShouldDependOnPropertyValues()
    {
        // Arrange
        var valueObject = new ValueObjectTest
        {
            FirstValue = "First Value",
            SecondValue = 42
        };

        // Act
        var firstHashCode = valueObject.GetHashCode();
        valueObject.FirstValue = "New Value";
        var secondHashCode = valueObject.GetHashCode();
        
        // Assert
        firstHashCode.Should().NotBe(secondHashCode);
    }

    [Fact]
    public void ValueObjectEqualsReturnsFalseWhenComparedToNull()
    {
        // Arrange
        var valueObject = new ValueObjectTest();

        // Act
        var result = valueObject.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ValueObjectEqualsReturnsFalseWhenComperedToOtherValueObjectType()
    {
        // Arrange
        var valueObject = new ValueObjectTest();
        var valueObject2 = new ValueObjectTest2();

        // Act
        // ReSharper disable once SuspiciousTypeConversion.Global
        var result = valueObject.Equals(valueObject2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ValueObjectEqualsReturnsTrueWhenObjectWithSameValuesAreCompared()
    {
        // Arrange
        var valueObject1 = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        var valueObject2 = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Different Value", 42)]
    [InlineData("Compared Value", -5)]
    [InlineData(null, default(int))]
    public void ValueObjectEqualsReturnsFalseWhenObjectWithDifferentValuesAreCompared(string firstValue, int secondValue)
    {
        // Arrange
        var valueObject = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        var valueObject2 = new ValueObjectTest
        {
            FirstValue = firstValue,
            SecondValue = secondValue
        };

        // Act
        var result = valueObject.Equals(valueObject2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ValueObjectEqualsReturnsTrueWhenInBothObjectsFieldIsNull()
    {
        // Arrange
        var valueObject = new ValueObjectTest
        {
            FirstValue = null,
            SecondValue = 42
        };

        var valueObject2 = new ValueObjectTest
        {
            FirstValue = null,
            SecondValue = 42
        };

        // Act
        var result = valueObject.Equals(valueObject2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValueObjectEqualsOperatorReturnsTrueForObjectsWithTheSameValues()
    {
        // Arrange
        var valueObject1 = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        var valueObject2 = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        // Act
        var result = valueObject1 == valueObject2;

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Different Value", 42)]
    [InlineData("Compared Value", -5)]
    [InlineData(null, default(int))]
    public void ValueObjectNotEqualsOperatorReturnsTrueWhenObjectWithDifferentValuesAreCompared(string firstValue, int secondValue)
    {
        // Arrange
        var valueObject = new ValueObjectTest
        {
            FirstValue = "Compared Value",
            SecondValue = 42
        };

        var valueObject2 = new ValueObjectTest
        {
            FirstValue = firstValue,
            SecondValue = secondValue
        };

        // Act
        var result = valueObject != valueObject2;

        // Assert
        result.Should().BeTrue();
    }
}
