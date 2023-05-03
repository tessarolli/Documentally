// <copyright file="GroupRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Data;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Group;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Utilities;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The Group Repository Implementation.
/// </summary>
public class GroupRepository : IGroupRepository
{
    private readonly ILogger logger;
    private readonly IUserRepository userRepository;
    private readonly DapperUtility db;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    /// <param name="userRepository">IUserRepository to inject.</param>
    public GroupRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<UserRepository> logger, IUserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
        this.db = new DapperUtility(postgresSqlConnectionFactory);
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> GetByIdAsync(GroupId id)
    {
        const string groupSql = @"
            SELECT 
                id, 
                name, 
                owner_id,
                created_at_utc
            FROM 
                groups
            WHERE 
                id = @Id;";

        var group = await db.QueryFirstOrDefaultAsync<GroupDto>(groupSql, new { Id = id.Value });

        if (group == null)
        {
            return Result.Fail<Group>($"Group with id {id} not found.");
        }

        const string membersSql = @"
            SELECT 
                group_id, 
                user_id
            FROM 
                group_members
            WHERE 
                group_id = @GroupId;";

        var members = await db.QueryAsync<GroupMemberDto>(membersSql, new { GroupId = id.Value });

        var mapped = CreateGroupResultFromGroupDto(group, members);
        if (mapped.IsSuccess)
        {
            return mapped.Value;
        }
        else
        {
            return Result.Fail(mapped.Errors);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<List<Group>>> GetAllAsync()
    {
        const string groupSql = @"
            SELECT 
                id, 
                name, 
                owner_id,
                created_at_utc
            FROM 
                groups";

        var groups = (await db.QueryAsync<GroupDto>(groupSql)).ToList();

        const string membersSql = @"
            SELECT 
                group_id, 
                user_id
            FROM 
                group_members";

        var members = (await db.QueryAsync<GroupMemberDto>(membersSql)).ToList();

        var result = new List<Group>();
        foreach (var group in groups)
        {
            var mapped = CreateGroupResultFromGroupDto(group, members);
            if (mapped.IsSuccess)
            {
                result.Add(mapped.Value);
            }
        }

        return Result.Ok(result);
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> AddAsync(Group group)
    {
        var insertSql = @"
            INSERT INTO 
                groups (name, owner_id)
            VALUES 
                (@Name, @OwnerId)
            RETURNING
                id";

        var parameters = new
        {
            group.Name,
            OwnerId = group.OwnerId.Value,
        };

        var x = await db.ExecuteScalarAsync(insertSql, parameters);

        await UpdateGroupUsersAsync(group, x);

        return await GetByIdAsync(new GroupId(x));
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> UpdateAsync(Group group)
    {
        logger.LogInformation("GroupRepository.UpdateAsync({group})", group.Name);

        var updateSql = @"
            UPDATE 
                groups
            SET 
                name = @Name, 
                owner_id = @OwnerId
            WHERE 
                id = @Id";

        var parameters = new
        {
            Id = group.Id.Value,
            group.Name,
            OwnerId = group.OwnerId.Value,
        };

        await db.ExecuteAsync(updateSql, parameters);

        await UpdateGroupUsersAsync(group);

        return await GetByIdAsync(group.Id);
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(GroupId groupId)
    {
        var parameter = new { GroupId = groupId.Value };

        var deleteDocumentAccessSql = @"
                DELETE FROM 
                    document_access
                WHERE 
                    group_id = @GroupId";

        await db.ExecuteAsync(deleteDocumentAccessSql, parameter);

        var deleteGroupMembersSql = @"
                DELETE FROM 
                    group_members
                WHERE 
                    group_id = @GroupId";

        await db.ExecuteAsync(deleteGroupMembersSql, parameter);

        var deleteGroupSql = @"
                DELETE FROM 
                    groups
                WHERE 
                    id = @GroupId";

        await db.ExecuteAsync(deleteGroupSql, parameter);

        return Result.Ok();
    }

    /// <summary>
    /// Method to update the Group Users in the database.
    /// </summary>
    /// <param name="group">The group to update.</param>
    /// <param name="newGroupId">In case of a newly added group, pass the Id here.</param>
    /// <returns>Awaitable task.</returns>
    private async Task UpdateGroupUsersAsync(Group group, long? newGroupId = null)
    {
        const string deleteSql = @"
            DELETE FROM 
                group_members
            WHERE 
                group_id = @GroupId";

        var deleteParameter = new
        {
            GroupId = group.Id.Value,
        };

        await db.ExecuteAsync(deleteSql, deleteParameter);

        const string insertSql = @"
            INSERT INTO 
                group_members (group_id, user_id) 
            VALUES 
                (@GroupId, @UserId)";

        var insertParamsEnumerable = group.MemberIds.Select(userId => new { UserId = userId.Value, GroupId = newGroupId ?? group.Id.Value });

        await db.ExecuteAsync(insertSql, insertParamsEnumerable);
    }

    /// <summary>
    /// Map the Group Data Transfer Object to a Result of Group.
    /// </summary>
    /// <param name="group">Group DTO.</param>
    /// <param name="members">List of Group Members DTOs.</param>
    /// <returns>The Result of Group.</returns>
    private Result<Group> CreateGroupResultFromGroupDto(GroupDto group, IEnumerable<GroupMemberDto> members)
    {
        var groupResult = Group.Create(
                new GroupId(group.id),
                group.name,
                new UserId(group.owner_id),
                members
                    .Where(x => x.group_id == group.id)
                    .Select(x => new UserId(x.user_id))
                    .ToList(),
                group.created_at_utc,
                new Lazy<Task<User>>(async () =>
                {
                    var getResult = await userRepository.GetByIdAsync(group.owner_id);
                    if (getResult.IsSuccess)
                    {
                        return getResult.Value;
                    }
                    else
                    {
                        return User.Empty;
                    }
                }),
                new Lazy<Task<List<User>>>(async () =>
                    await userRepository.GetByIdsAsync(members.Where(x => x.group_id == group.id).Select(x => x.user_id).ToArray())));

        return groupResult;
    }
}
