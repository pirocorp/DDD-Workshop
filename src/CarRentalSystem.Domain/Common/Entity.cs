namespace CarRentalSystem.Domain.Common;

using System;

public abstract class Entity<TId> where TId : struct
{
    public TId Id { get; protected set; } = default;

    public override bool Equals(object? obj)
    {
        // base is derived = true
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // base != derived
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        // objects with default id are not equal 
        if (this.Id.Equals(default(TId)) || other.Id.Equals(default(TId)))
        {
            return false;
        }

        return this.Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<TId>? first, Entity<TId>? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(Entity<TId>? first, Entity<TId>? second) => !(first == second);

    public override int GetHashCode() => HashCode.Combine(this.GetType().ToString(), this.Id);
}

