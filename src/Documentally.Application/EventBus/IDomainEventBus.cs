// <copyright file="IDomainEventBus.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;

namespace Documentally.Application.EventBus;

/// <summary>
/// Factory for dispatching Domain Events.
/// </summary>
public interface IDomainEventBus
{
    /// <summary>
    /// Publishes all events in the queue for the referenced entity.
    /// </summary>
    /// <param name="entity">The entity instance for publishind the Domain Events.</param>
    public void DispatchDomainEvents(IEntity entity);
}
