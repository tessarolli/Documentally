// <copyright file="Entity.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.BaseClasses.DDD;

/// <summary>
/// An abstract class that should be implemented to represent an Entity of the Domain.
/// </summary>
/// <typeparam name="TId">The Type of the Entity's Id parameter.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The Entity's Id.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets the Identificator of this entity.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Equality operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have the same Ids.</returns>
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Not Equal operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have different Ids.</returns>
    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    /// <inheritdoc/>
    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }
}