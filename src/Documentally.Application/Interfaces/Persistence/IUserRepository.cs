// <copyright file="IUserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Entities;
using FluentResults;

namespace Documentally.Application.Interfaces.Persistence;

/// <summary>
/// Interface for the User Repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get an element from the User Repository by it's id.
    /// </summary>
    /// <param name="id">The Id of the element to fetch.</param>
    /// <returns>The element instance, if it exists.</returns>
    User? GetById(Guid id);

    /// <summary>
    /// Get an element from the User Repository by it's email address.
    /// </summary>
    /// <param name="email">The email address of the element to fetch.</param>
    /// <returns>The element instance, if it exists.</returns>
    User? GetByEmail(string email);

    /// <summary>
    /// Add an User to the user Repository.
    /// </summary>
    /// <param name="user">The User instance to add to the repository.</param>
    /// <returns>An Result indicating if the user was successfully added or not.</returns>
    Result<User> Add(User user);
}
