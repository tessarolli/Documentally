﻿// <copyright file="Group.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using Documentally.Domain.GroupAggregate.Validators;
using Documentally.Domain.GroupAggregate.ValueObjects;
using Documentally.Domain.UserAggregate;
using Documentally.Domain.UserAggregate.ValueObjects;
using FluentResults;

namespace Documentally.Domain.GroupAggregate;

/// <summary>
/// Group Entity.
/// </summary>
public sealed class Group : Entity<GroupId>
{
    private readonly List<UserId> memberIds = new ();
    private readonly Lazy<Task<List<User>>> members = new ();

    private Group(GroupId id, Lazy<Task<User>>? owner, Lazy<Task<List<User>>>? members, List<UserId> memberIds)
        : base(id)
    {
        Id = id;
        this.memberIds = memberIds;

        Owner = owner ?? new Lazy<Task<User>>(async () =>
        {
            await Task.CompletedTask;
            return User.Empty;
        });

        this.members = members ?? new Lazy<Task<List<User>>>(async () =>
        {
            await Task.CompletedTask;
            return new List<User>();
        });
    }

    /// <summary>
    /// Gets Group's Name.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets the Group's Owner Id.
    /// </summary>
    public UserId OwnerId { get; init; } = null!;

    /// <summary>
    /// Gets Group Owner.
    /// </summary>
    public Lazy<Task<User>> Owner { get; init; } = null!;

    /// <summary>
    /// Gets the Group's Members Id list.
    /// </summary>
    public IReadOnlyList<UserId> MemberIds => memberIds.ToList();

    /// <summary>
    /// Gets Group's List of Members.
    /// </summary>
    public IReadOnlyList<User> Members => members.Value.Result.AsReadOnly();

    /// <summary>
    /// Gets Group's Creation Date on utc.
    /// </summary>
    public DateTime CreatedAtUtc { get; init; }

    /// <summary>
    /// Group Creation Factory.
    /// </summary>
    /// <param name="id">Group Id.</param>
    /// <param name="name">Name of the Group.</param>
    /// <param name="ownerId">The Owner Id.</param>
    /// <param name="memberIds">The group members Ids.</param>
    /// <param name="createdAtUtc">The group creation date.</param>
    /// <param name="owner">The Lazy Load Implementation for the Owner.</param>
    /// <param name="members">The Lazy Load Implementation for the Group Members.</param>
    /// <returns>Group Domain Instance.</returns>
    public static Result<Group> Create(GroupId? id, string name, UserId? ownerId = null, List<UserId>? memberIds = null, DateTime? createdAtUtc = null, Lazy<Task<User>>? owner = null, Lazy<Task<List<User>>>? members = null)
    {
        var group = new Group(id ?? new GroupId(), owner, members, memberIds ?? new List<UserId>())
        {
            Name = name,
            OwnerId = ownerId ?? new UserId(null),
            CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow.AddYears(-100),
        };

        var validationResult = group.Validate();
        if (validationResult.IsSuccess)
        {
            return Result.Ok(group);
        }
        else
        {
            return Result.Fail(validationResult.Errors);
        }
    }

    /// <summary>
    /// Adds an Member to this Group.
    /// </summary>
    /// <param name="memberId">The member to add.</param>
    public void AddMember(UserId memberId)
    {
        // check if user is already a member
        if (memberIds.Contains(memberId))
        {
            return;
        }

        memberIds.Add(memberId);
    }

    /// <summary>
    /// Removes a Member from this Group.
    /// </summary>
    /// <param name="memberId">The Member to remove.</param>
    public void RemoveMember(UserId memberId)
    {
        memberIds.Remove(memberId);
    }

    /// <inheritdoc/>
    protected override object GetValidator() => new GroupValidator();
}
