// <copyright file="User.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.BaseClasses.DDD;
using Documentally.Domain.ValueObjects;

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
    public string Password { get; private set; } = null!;

    /// <summary>
    /// Gets User's Last Name.
    /// </summary>
    public string Role { get; private set; } = null!;

    /// <summary>
    /// Creates a new User.
    /// </summary>
    /// <param name="firstName">User's First Name.</param>
    /// <param name="lastName">User's Last Name.</param>
    /// <param name="email">User's Email.</param>
    /// <param name="password">User's Password.</param>
    /// <returns>New User's instance.</returns>
    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new User(new UserId())
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
        };
    }
}