// <copyright file="UserRegisteredDomainEventHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.UserAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Documentally.Application.Authentication.DomainEventHandlers;

/// <summary>
/// UserRegisteredDomainEventHandler.
/// </summary>
public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly ILogger<UserRegisteredDomainEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisteredDomainEventHandler"/> class.
    /// </summary>
    /// <param name="logger">ILogger injected.</param>
    public UserRegisteredDomainEventHandler(ILogger<UserRegisteredDomainEventHandler> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task Handle(UserCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handle UserRegisteredDomainEvent");

        await Task.CompletedTask;
    }
}
