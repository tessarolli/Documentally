// <copyright file="User.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.BaseClasses.DDD;
using Documentally.Domain.Enums;
using Documentally.Domain.ValueObjects;
using FluentResults;

namespace Documentally.Domain.Entities;

/// <summary>
/// User Entity.
/// </summary>
public class User : Entity<UserId>
{
    private User(UserId id)
        : base(id)
    {
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
    /// Creates a new User.
    /// </summary>
    /// <param name="firstName">User's First Name.</param>
    /// <param name="lastName">User's Last Name.</param>
    /// <param name="email">User's Email.</param>
    /// <param name="password">User's Password.</param>
    /// <returns>New User's instance or error.</returns>
    public static Result<User> Create(string firstName, string lastName, string email, string password)
    {
        var passwordResult = Password.Create(password);

        if (passwordResult.IsFailed)
        {
            return Result.Fail(passwordResult.Errors);
        }

        return new User(new UserId())
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = passwordResult.Value,
            Role = Roles.User,
        };
    }
}