// <copyright file="User.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.Common.DDD;
using Documentally.Domain.Enums;
using Documentally.Domain.User.ValueObjects;
using FluentResults;
using FluentValidation;

namespace Documentally.Domain.User;

/// <summary>
/// User Entity.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    /// <summary>
    /// Gets a Empty User instance.
    /// </summary>
    public static readonly User Empty = new (new UserId(null));

    private User(UserId id)
        : base(id)
    {
        Id = id;
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
    /// Gets User's Email.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Gets User's Password.
    /// </summary>
    public Password Password { get; private set; } = null!;

    /// <summary>
    /// Gets User's Role.
    /// </summary>
    public Roles Role { get; private set; }

    /// <summary>
    /// Gets User's Creation Date on utc.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

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
    /// <param name="createdOnUtc">User's creation date.</param>
    /// <param name="passwordHasher">Password hasher.</param>
    /// <returns>New User's instance or error.</returns>
    public static Result<User> Create(long? id, string firstName, string lastName, string email, string password, Roles role = Roles.User, DateTime? createdOnUtc = null, IPasswordHasher? passwordHasher = null)
    {
        Password hashedPassword;
        try
        {
            hashedPassword = new Password(password, passwordHasher);
        }
        catch (Exception)
        {
            return Result.Fail("Invalid Password");
        }

        var user = new User(new UserId(id))
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = hashedPassword,
            Role = role,
            IsRegistered = id is not null,
            CreatedAtUtc = createdOnUtc ?? DateTime.UtcNow.AddYears(-100),
        };

        var validationResult = user.Validate();
        if (validationResult.IsSuccess)
        {
            return user;
        }
        else
        {
            return Result.Fail(validationResult.Errors);
        }
    }

    /// <inheritdoc/>
    protected override IValidator GetValidator() => null!;
}