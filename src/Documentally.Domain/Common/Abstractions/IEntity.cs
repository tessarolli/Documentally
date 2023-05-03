// <copyright file="IEntity.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Common.Abstractions;

/// <summary>
/// IEntity interface.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets Entity's Domain Event Collection.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Gets the Id of the Entity.
    /// </summary>
    /// <returns>Returns the Entity's Id Value Object.</returns>
    object GetId();

    /// <summary>
    /// Add an Domain Event to Entity's list of Domain Events.
    /// </summary>
    /// <param name="event">The event to add.</param>
    void RaiseDomainEvent(IDomainEvent @event);

    /// <summary>
    /// Removes all Domain Events from Entity's list of Domain Events.
    /// </summary>
    void ClearDomainEvents();
}
