// <copyright file="DomainEventBus.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Collections.Concurrent;
using Documentally.Domain.Common.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Documentally.Application.EventBus;

/// <summary>
/// This is just a proof of concept implementation of a simple Domain Event Dispatcher (Publishing with MediatR).
/// In a real world scenario, this would be implemented using
/// RabbitMQ/Kafka/Azure Service Bus, or some enterprise level messaging service.
/// </summary>
public class DomainEventBus : IDomainEventBus
{
    private readonly ConcurrentQueue<IDomainEvent> domainEvents = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventBus"/> class.
    /// </summary>
    /// <param name="mediator">IMediator being injected.</param>
    /// <param name="logger">ILogger being injected.</param>
    public DomainEventBus(IMediator mediator, ILogger<DomainEventBus> logger)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                while (domainEvents.TryDequeue(out var domainEvent))
                {
                    try
                    {
                        await mediator.Publish(domainEvent);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error publishing domain event {EventType}", domainEvent.GetType().Name);
                        throw;
                    }
                }

                await Task.Delay(TimeSpan.FromMilliseconds(10));
            }
        });
    }

    /// <inheritdoc/>
    public void DispatchDomainEvents(IEntity entity)
    {
        foreach (var domainEvent in entity.DomainEvents)
        {
            domainEvents.Enqueue(domainEvent);
        }

        entity.ClearDomainEvents();
    }
}