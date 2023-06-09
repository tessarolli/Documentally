﻿// <copyright file="IGroupRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Data.Common;
using Documentally.Domain.Group;
using Documentally.Domain.Group.ValueObjects;
using FluentResults;

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
    /// <param name="transaction">The Transaction.</param>
    /// <returns>A Result with the Group Aggregate, or a error message.</returns>
    Task<Result<Group>> GetByIdAsync(GroupId id, DbTransaction? transaction = null);

    /// <summary>
    /// Gets a List of all Groups from the repository.
    /// </summary>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<List<Group>>> GetAllAsync();

    /// <summary>
    /// Add an Group into the Repository.
    /// </summary>
    /// <param name="group">The Group to Add.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<Group>> AddAsync(Group group);

    /// <summary>
    /// Update the Group in the Repository.
    /// </summary>
    /// <param name="group">The Group to Update.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result<Group>> UpdateAsync(Group group);

    /// <summary>
    /// Remove the Group from the Repository.
    /// </summary>
    /// <param name="groupId">The Group to Remove.</param>
    /// <returns>A Result indicating the status of this operation.</returns>
    Task<Result> RemoveAsync(GroupId groupId);
}
