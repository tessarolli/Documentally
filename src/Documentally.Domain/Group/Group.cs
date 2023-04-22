// <copyright file="Group.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using Documentally.Domain.Enums;
using Documentally.Domain.User.ValueObjects;

namespace Documentally.Domain.Group;

/// <summary>
/// Group Entity.
/// </summary>
public sealed class Group : Entity
{
    private Group(GroupId id)
        : base(id?.Value)
    {
    }

    /// <summary>
    /// Gets Group's First Name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets Group's Last Name.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Gets Group's Last Name.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Gets Group's Last Name.
    /// </summary>
    public Password Password { get; private set; } = null!;

    /// <summary>
    /// Gets Group's Last Name.
    /// </summary>
    public Roles Role { get; private set; }

    /// <summary>
    /// Gets Group's Creation Date on utc.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }
}
