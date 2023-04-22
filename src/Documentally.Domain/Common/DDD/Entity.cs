// <copyright file="Entity.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;

namespace Documentally.Domain.Common.DDD;

/// <summary>
/// An abstract class that should be implemented to represent an Entity of the Domain.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> domainEvents = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> class.
    /// </summary>
    /// <param name="id">Entity Id.</param>
    protected Entity(long? id)
    {
        Id = id ?? 0;
    }

    /// <summary>
    /// Gets The DomainEvents List.
    /// </summary>
    public List<IDomainEvent> DomainEvents { get => domainEvents.ToList(); }

    /// <summary>
    /// Gets or sets the Identificator of this entity.
    /// </summary>
    public long Id { get; protected set; }

    /// <summary>
    /// Equality operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have the same Ids.</returns>
    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Not Equal operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have different Ids.</returns>
    public static bool operator !=(Entity left, Entity right)
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
        return obj is Entity entity && Id.Equals(entity.Id);
    }

    /// <inheritdoc/>
    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// Clear Domain Events.
    /// </summary>
    /// <param name="event">The domain event to be cleared.</param>
    public void ClearDomainEvent(IDomainEvent @event)
    {
        domainEvents.Remove(@event);
    }

    /// <summary>
    /// Method for raising Domain Events.
    /// </summary>
    /// <param name="event">The domain event to be raised.</param>
    protected void RaiseDomainEvent(IDomainEvent @event)
    {
        domainEvents.Add(@event);
    }
}