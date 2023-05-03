// <copyright file="UserCreatedDomainEventHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.User.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Documentally.Application.Authentication.DomainEventHandlers;

/// <summary>
/// UserCreatedDomainEventHandler.
/// </summary>
public sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly ILogger<UserCreatedDomainEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreatedDomainEventHandler"/> class.
    /// </summary>
    /// <param name="logger">ILogger injected.</param>
    public UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task Handle(UserCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handle UserCreatedDomainEvent");

        /*
         * This is just a simple User Entity Created Event, there's nothing much to do here.
         */

        await Task.CompletedTask;
    }
}
