namespace CarRentalSystem.Domain.Common;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class Enumeration : IComparable
{
    private static readonly ConcurrentDictionary<Type, IEnumerable<object>> EnumCache = new();

    protected Enumeration(int value, string name)
    {
        this.Value = value;
        this.Name = name;
    }

    public string Name { get; }

    public int Value { get; }

    public override string ToString() => this.Name;

    public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration
    {
        var type = typeof(TEnumeration);

        var values = EnumCache.GetOrAdd(
            type,
            _ => type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null)!));

        return values.Cast<TEnumeration>();
    }

    public static TEnumeration FromValue<TEnumeration>(int value) where TEnumeration : Enumeration
        => Parse<TEnumeration, int>(value, "value", item => item.Value == value);

    public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration
        => Parse<TEnumeration, string>(name, "name", item => item.Name == name);

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = this.GetType() == obj.GetType();
        var valueMatches = this.Value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => HashCode.Combine(this.GetType().ToString(), this.Value);

    public int CompareTo(object? other) => this.Value.CompareTo(((Enumeration)other!).Value);

    private static TEnumeration Parse<TEnumeration, TValue>(
        TValue value, 
        string description, 
        Func<TEnumeration, bool> predicate) 
        where TEnumeration : Enumeration
    {
        var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(TEnumeration)}");
        }

        return matchingItem;
    }
}
