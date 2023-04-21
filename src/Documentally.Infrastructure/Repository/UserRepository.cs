// <copyright file="UserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Entities;
using FluentResults;

namespace Documentally.Infrastructure.Repository;

/// <summary>
/// The User Repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private static readonly List<User> USERS = new ();

    /// <inheritdoc/>
    public Result<User> Add(User user)
    {
        USERS.Add(user);

        var result = new Result<User>();

        return result.WithSuccess("User add to database");
    }

    /// <inheritdoc/>
    public User? GetByEmail(string email)
    {
        var user = USERS.Where(x => x.Email.Equals(email)).FirstOrDefault();

        return user;
    }

    /// <inheritdoc/>
    public User? GetById(Guid id)
    {
        var user = USERS.Where(x => x.Id.Value.Equals(id)).FirstOrDefault();

        return user;
    }
}
