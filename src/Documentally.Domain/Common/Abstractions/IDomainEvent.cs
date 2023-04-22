// <copyright file="IDomainEvent.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using MediatR;

namespace Documentally.Domain.Common.Abstractions;

/// <summary>
/// Represents an Domain Event.
/// </summary>
public interface IDomainEvent : INotification
{
}
