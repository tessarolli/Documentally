// <copyright file="IUserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Abstractions.Repositories;

/// <summary>
/// Interface for the User Repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get an element from the User Repository by it's id.
    /// </summary>
    /// <param name="id">The Id of the element to fetch.</param>
    /// <returns>The Result of the GetById.</returns>
    Task<Result<User>> GetByIdAsync(long id);

    /// <summary>
    /// Gets a List of User instances from an array of Ids.
    /// </summary>
    /// <param name="ids">The Array of Ids.</param>
    /// <returns>A List of User.</returns>
    Task<List<User>> GetByIdsAsync(long[] ids);

    /// <summary>
    /// Get an element from the User Repository by it's email address.
    /// </summary>
    /// <param name="email">The email address of the element to fetch.</param>
    /// <returns>The Result of GetByEmail.</returns>
    Task<Result<User>> GetByEmailAsync(string email);

    /// <summary>
    /// Gets a List of all Users from the repository.
    /// </summary>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<List<User>>> GetAllAsync();

    /// <summary>
    /// Add an User to the user Repository.
    /// </summary>
    /// <param name="user">The User instance to add to the repository.</param>
    /// <returns>An Result indicating if the user was successfully added or not.</returns>
    Task<Result<User>> AddAsync(User user);

    /// <summary>
    /// Update the User in the Repository.
    /// </summary>
    /// <param name="user">The User to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<User>> UpdateAsync(User user);

    /// <summary>
    /// Remove the User from the Repository.
    /// </summary>
    /// <param name="userId">The User to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result> RemoveAsync(UserId userId);
}
