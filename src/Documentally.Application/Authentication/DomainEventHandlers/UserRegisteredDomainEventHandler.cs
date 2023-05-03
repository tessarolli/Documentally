// <copyright file="UserRegisteredDomainEventHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.User.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Documentally.Application.Authentication.DomainEventHandlers;

/// <summary>
/// UserRegisteredDomainEventHandler.
/// </summary>
public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
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
    public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handle UserRegisteredDomainEvent");

        /*
         * After this Domain Event is handled, we can fire relative Integration Events,
         * for example, an Integration Event to send an
         * welcome/validation e-mail to the registered user e-mail.
         * This can be performed on this service, or on a different service,
         * by using an EventBus implementation.
         * But, in this case, we will do nothing,
         * as this is just a proof of concept implementation.
        */

        await Task.CompletedTask;
    }
}
