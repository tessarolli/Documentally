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

        var group = await db.QueryFirstOrDefaultAsync<GroupDto>(groupSql, new { Id = id.Value }, System.Data.CommandType.Text);

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

        var members = await db.QueryAsync<GroupMemberDto>(membersSql, new { GroupId = id.Value }, System.Data.CommandType.Text);

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

        var x = await db.ExecuteScalarAsync(insertSql, parameters, System.Data.CommandType.Text);

        await AddUserToGroupAsync(new GroupId(x), group.OwnerId);

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

        await db.ExecuteAsync(updateSql, parameters, System.Data.CommandType.Text);

        return await GetByIdAsync(group.Id);
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(GroupId groupId)
    {
        var transaction = await db.BeginTransactionAsync();

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    document_access
                WHERE 
                    group_id = @GroupId";

            var x = await db.ExecuteAsync(deleteSql, new { GroupId = groupId.Value }, transaction: transaction);
        }
        catch (Exception)
        {
            transaction.Rollback();

            throw;
        }

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    group_members
                WHERE 
                    group_id = @GroupId";

            var x = await db.ExecuteAsync(deleteSql, new { GroupId = groupId.Value }, transaction: transaction);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    groups
                WHERE 
                    id = @GroupId";

            var y = await db.ExecuteAsync(deleteSql, new { GroupId = groupId.Value }, CommandType.Text, transaction);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }

        try
        {
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, string.Empty);
        }

        return Result.Ok();
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
    public async Task<Result<Group>> AddUserToGroupAsync(GroupId groupId, UserId userId)
    {
        const string insertSql = @"
            INSERT INTO 
                group_members (group_id, user_id) 
            VALUES 
                (@GroupId, @UserId);";

        var parameters = new
        {
            GroupId = groupId.Value,
            UserId = userId.Value,
        };

        await db.ExecuteAsync(insertSql, parameters, System.Data.CommandType.Text);

        return await GetByIdAsync(groupId);
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> RemoveUserFromGroupAsync(GroupId groupId, UserId userId)
    {
        const string deleteSql = @"
            DELETE FROM 
                group_members
            WHERE 
                group_id = @GroupId 
                AND user_id = @UserId;";

        var parameters = new
        {
            GroupId = groupId.Value,
            UserId = userId.Value,
        };

        await db.ExecuteAsync(deleteSql, parameters);

        return await GetByIdAsync(groupId);
    }

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
