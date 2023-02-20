namespace CarRentalSystem.Domain.Tests.Common;

using System;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Models.Dealers;

using FluentAssertions;
using Xunit;

public class EntityTests
{
    private class EntityTest : Entity<Guid>
    { }

    [Fact]
    public void EntityEqualsMethodReturnsTrueForObjectsWithTheSameId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var mockA = new EntityTest().SetId(guid);
        var mockB = new EntityTest().SetId(guid);

        // Act
        var result = mockA.Equals(mockB);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityEqualsMethodReturnsFalseForObjectsWithDifferentIds()
    {
        // Arrange
        var mockA = new EntityTest();
        mockA.SetId(Guid.NewGuid());

        var mockB = new EntityTest();
        mockB.SetId(Guid.NewGuid());

        // Act
        var result = mockA.Equals(mockB);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEqualsMethodReturnsFalseForNonEntityObjects()
    {
        // Arrange
        var entity = new EntityTest();

        // Act
        var result = entity.Equals(new object());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEqualsMethodReturnsTrueForSameReference()
    {
        // Arrange
        var entity = new EntityTest();

        // Act
        // ReSharper disable once EqualExpressionComparison
        var result = entity.Equals(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityEqualsMethodReturnsFalseForDifferentEntityTypes()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var entity = new EntityTest().SetId(guid);
        var dealer = new Dealer("Valid name", "+359123456789").SetId(guid);

        // Act
        // ReSharper disable once SuspiciousTypeConversion.Global
        var result = entity.Equals(dealer);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEqualsMethodReturnsFalseForDefaultIds()
    {
        // Arrange
        var entityWithDefaultGuid = new EntityTest();
        var entity = new EntityTest().SetId(Guid.NewGuid());

        // Act
        var result1 = entityWithDefaultGuid.Equals(entity);
        var result2 = entity.Equals(entityWithDefaultGuid);

        // Assert
        result1.Should().BeFalse();
        result2.Should().BeFalse();
    }

    [Fact]
    public void EntityEqualsOperatorReturnsTrueForTwoNulls()
    {
        // Arrange
        EntityTest? entityA = null;
        EntityTest? entityB = null;

        // Act
        var result = entityA == entityB;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityEqualsOperatorReturnsFalseIfOneIsNull()
    {
        // Arrange
        EntityTest? entityA = null;
        var entityB = new EntityTest();

        // Act
        var result1 = entityA == entityB;
        var result2 = entityB == entityA;

        // Assert
        result1.Should().BeFalse();
        result2.Should().BeFalse();
    }

    [Fact]
    public void EntityEqualsOperatorReturnsTrueForObjectsWithTheSameId()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var mockA = new EntityTest().SetId(guid);
        var mockB = new EntityTest().SetId(guid);

        // Act
        var result = mockA == mockB;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityNotEqualsOperatorReturnsTrueForObjectsWithDifferentIds()
    {
        // Arrange
        var mockA = new EntityTest().SetId(Guid.NewGuid());
        var mockB = new EntityTest().SetId(Guid.NewGuid());

        // Act
        var result = mockA != mockB;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityHashCodeIsDifferentFromTypeHashCodeAndIdHashCode()
    {
        // Arrange
        var entity = new EntityTest();
        var typeHashCode = entity.GetType().GetHashCode();
        var idHashCode = entity.Id.GetHashCode();
        var entityHashCode = entity.GetHashCode();

        // Act
        var result1 = entityHashCode != typeHashCode;
        var result2 = entityHashCode != idHashCode;

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }
}

internal static class EntityExtensions
{
    public static Entity<TId> SetId<TId>(this Entity<TId> entity, TId id)
        where TId : struct
    {
        entity
            .GetType()
            .BaseType
            ?.GetProperty(nameof(Entity<TId>.Id))
            ?.GetSetMethod(true)
            ?.Invoke(entity, new object[] { id });

        return entity;
    }
}