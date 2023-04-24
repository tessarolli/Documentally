// <copyright file="GroupId.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;

namespace Documentally.Domain.Group.ValueObjects;

/// <summary>
/// Group Id Value Object.
/// </summary>
public sealed class GroupId : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupId"/> class.
    /// </summary>
    /// <param name="id">Id value if exists.</param>
    public GroupId(long? id = null)
    {
        if (id is null)
        {
            Value = 0;
        }
        else
        {
            Value = id.Value;
        }
    }

    /// <summary>
    /// Gets the Group ID.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Method required for comparing value objects.
    /// </summary>
    /// <returns>An ienumerable with all the properties of the value object.</returns>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
