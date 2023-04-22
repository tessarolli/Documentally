// <copyright file="MediatorExtension.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using MediatR;

namespace Documentally.Application.Extensions;

/// <summary>
/// Mediator Extension.
/// </summary>
public static class MediatorExtension
{
    /// <summary>
    /// Dispatch Domain Events for the Entity.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    /// <param name="mediator">mediator instance.</param>
    /// <param name="entity">entity instance.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task DispatchDomainEventsAsync<T>(this IMediator mediator, T entity)
        where T : Entity
    {
        var tasks = entity.DomainEvents
            .Select(async (domainEvent) =>
            {
                entity.ClearDomainEvent(domainEvent);

                await mediator.Publish(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}
