// <copyright file="AggregateRoot.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.BaseClasses.DDD;

/// <summary>
/// Base class for Aggregate Roots.
/// </summary>
/// <typeparam name="TId">The Type for the Id property.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">The Id for this instance.</param>
    protected AggregateRoot(TId id)
        : base(id)
    {
    }
}