// <copyright file="IGroupRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.GroupAggregate.ValueObjects;
using Documentally.Domain.UserAggregate.ValueObjects;
using FluentResults;
using Group_ = Documentally.Domain.GroupAggregate.Group;

namespace Documentally.Application.Abstractions.Repositories;

/// <summary>
/// Group Repository Interface.
/// </summary>
public interface IGroupRepository
{
    /// <summary>
    /// Get an group aggregate by Id.
    /// </summary>
    /// <param name="id">The Group Id.</param>
    /// <returns>A Result with the Group Aggregate, or a error message.</returns>
    Task<Result<Group_>> GetByIdAsync(GroupId id);

    /// <summary>
    /// Add an Group into the Repository.
    /// </summary>
    /// <param name="group">The Group to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<Group_>> AddAsync(Group_ group);

    /// <summary>
    /// Update the Group in the Repository.
    /// </summary>
    /// <param name="group">The Group to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<Group_>> UpdateAsync(Group_ group);

    /// <summary>
    /// Remove the Group from the Repository.
    /// </summary>
    /// <param name="groupId">The Group to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result> RemoveAsync(GroupId groupId);

    /// <summary>
    /// Gets a List of all Groups from the repository.
    /// </summary>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<List<Group_>>> GetAllAsync();

    /// <summary>
    /// Adds an User to the Group in the Repository.
    /// </summary>
    /// <param name="groupId">The Group Identifier.</param>
    /// <param name="userId">The User Identifier.</param>
    /// <returns>A Result indicating the status of this operation and updated instance of the Group Aggregate.</returns>
    Task<Result<Group_>> AddUserToGroupAsync(GroupId groupId, UserId userId);

    /// <summary>
    /// Removes an User from the Group in the Repository.
    /// </summary>
    /// <param name="groupId">The Group Identifier.</param>
    /// <param name="userId">The User Identifier.</param>
    /// <returns>A Result indicating the status of this operation and updated instance of the Group Aggregate.</returns>
    Task<Result<Group_>> RemoveUserFromGroupAsync(GroupId groupId, UserId userId);
}
