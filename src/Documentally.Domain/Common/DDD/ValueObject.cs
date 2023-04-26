// <copyright file="ValueObject.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Diagnostics;

namespace Documentally.Domain.Common.DDD;

/// <summary>
/// An abstract class that should be implemented to represent an Value Object.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Equality operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have the same Ids.</returns>
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Not Equal operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have different Ids.</returns>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Abstract method to get all properties from the value object to calculate equality.
    /// Disclaimer: An value object is only equal when all of its properties have equal values.
    /// </summary>
    /// <returns>Should be implemented to yield return propertyName for each property of the value object.</returns>
    public abstract IEnumerable<object> GetEqualityComponents();

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
    }

    /// <inheritdoc/>
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }
}