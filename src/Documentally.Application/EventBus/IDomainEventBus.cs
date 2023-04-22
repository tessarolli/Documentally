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
    /// Adds a Domain Event to the Dispatcher Queue.
    /// </summary>
    /// <param name="domainEvent">Event being dispatched.</param>
    public void DispatchDomainEvent(IDomainEvent domainEvent);
}
