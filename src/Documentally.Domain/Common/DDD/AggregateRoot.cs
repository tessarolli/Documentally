// <copyright file="AggregateRoot.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Common.DDD;

/// <summary>
/// Base class for Aggregate Roots.
/// </summary>
public abstract class AggregateRoot : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
    /// </summary>
    /// <param name="id">Entity Id.</param>
    protected AggregateRoot(long? id)
        : base(id)
    {
    }
}