// <copyright file="User.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using Documentally.Domain.Enums;
using Documentally.Domain.User.Events;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Domain.User;

/// <summary>
/// User Entity.
/// </summary>
public sealed class User : AggregateRoot
{
    private User(long? id)
        : base(id)
    {
        Id = id ?? 0;
    }

    /// <summary>
    /// Gets User's First Name.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Gets User's Last Name.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Gets User's Last Name.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Gets User's Last Name.
    /// </summary>
    public Password Password { get; private set; } = null!;

    /// <summary>
    /// Gets User's Last Name.
    /// </summary>
    public Roles Role { get; private set; }

    /// <summary>
    /// Gets User's Creation Date on utc.
    /// </summary>
    public DateTime? CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this user is registered or not.
    /// </summary>
    public bool IsRegistered { get; private set; }

    /// <summary>
    /// Creates a new User Entity instance.
    /// </summary>
    /// <param name="id">User's id, can be null.</param>
    /// <param name="firstName">User's First Name.</param>
    /// <param name="lastName">User's Last Name.</param>
    /// <param name="email">User's Email.</param>
    /// <param name="password">User's Password.</param>
    /// <param name="role">User's Role. can be null.</param>
    /// <returns>New User's instance or error.</returns>
    public static Result<User> Create(long? id, string firstName, string lastName, string email, string password, Roles role = Roles.User, DateTime? createdOnUtc = null)
    {
        var passwordResult = Password.Create(password);

        if (passwordResult.IsFailed)
        {
            return Result.Fail(passwordResult.Errors);
        }

        return new User(id)
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = passwordResult.Value,
            Role = role,
            IsRegistered = id is not null,
            CreatedAtUtc = createdOnUtc,
        };
    }

    /// <summary>
    /// Initiates a User Registration Process.
    /// </summary>
    public void RaiseUserCreatedDomainEvent()
    {
        IsRegistered = true;
        RaiseDomainEvent(new UserCreatedDomainEvent(this));
    }
}