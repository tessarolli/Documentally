// <copyright file="AggregateRoot.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Common.DDD;

/// <summary>
/// Base class for Aggregate Roots.
/// </summary>
/// <typeparam name="TId">The Type of the Id value object.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">Entity Id.</param>
    protected AggregateRoot(TId id)
        : base(id)
    {
    }
}