// <copyright file="GroupRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Group;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The Group Repository Implementation.
/// </summary>
public class GroupRepository : IGroupRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;
    private readonly ILogger logger;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    /// <param name="userRepository">IUserRepository to inject.</param>
    public GroupRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<UserRepository> logger, IUserRepository userRepository)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
        this.logger = logger;
        this.userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> GetByIdAsync(GroupId id)
    {
        logger.LogInformation("GroupRepository.GetByIdAsync({id})", id);

        using var connection = postgresSqlConnectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

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

        var group = await connection.QueryFirstOrDefaultAsync<GroupDto>(groupSql, new { Id = id.Value });

        if (group == null)
        {
            await transaction.RollbackAsync();

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

        var members = (await connection.QueryAsync<GroupMemberDto>(membersSql, new { GroupId = id.Value })).ToList();

        await transaction.CommitAsync();

        var mapped = MapDtoToGroup(group, members);
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
        logger.LogInformation("GroupRepository.AddAsync({group})", group.Name);

        var insertSql = @"
            INSERT INTO 
                groups (name, owner_id)
            VALUES 
                (@Name, @OwnerId)
            RETURNING
                id;";

        var parameters = new
        {
            group.Name,
            OwnerId = group.OwnerId.Value,
        };

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var x = await connection.ExecuteScalarAsync<long>(insertSql, parameters);

            await connection.CloseAsync();

            if (x > 0)
            {
                await AddUserToGroupAsync(new GroupId(x), group.OwnerId);

                return await GetByIdAsync(new GroupId(x));
            }

            return Result.Fail("Group Insertion into database failed");
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
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
                id = @Id;";

        var parameters = new
        {
            Id = group.Id.Value,
            group.Name,
            OwnerId = group.OwnerId.Value,
        };

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var x = await connection.ExecuteAsync(updateSql, parameters);

            await connection.CloseAsync();

            return await GetByIdAsync(new GroupId(x));
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(GroupId groupId)
    {
        logger.LogInformation("GroupRepository.RemoveAsync({group})", groupId);

        using var connection = postgresSqlConnectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    document_access
                WHERE 
                    group_id = @GroupId;";

            var x = await connection.ExecuteAsync(deleteSql, new { GroupId = groupId.Value });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return Result.Fail(ex.GetPrettyMessage(logger));
        }

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    group_members
                WHERE 
                    group_id = @GroupId;";

            var x = await connection.ExecuteAsync(deleteSql, new { GroupId = groupId.Value });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return Result.Fail(ex.GetPrettyMessage(logger));
        }

        try
        {
            var deleteSql = @"
                DELETE FROM 
                    groups
                WHERE 
                    id = @GroupId;";

            var y = await connection.ExecuteAsync(deleteSql, new { GroupId = groupId.Value });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return Result.Fail(ex.GetPrettyMessage(logger));
        }

        await transaction.CommitAsync();

        return Result.Ok();
    }

    /// <inheritdoc/>
    public async Task<Result<List<Group>>> GetAllAsync()
    {
        logger.LogInformation("GroupRepository.GetAllAsync()");

        using var connection = postgresSqlConnectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        const string groupSql = @"
            SELECT 
                id, 
                name, 
                owner_id,
                created_at_utc
            FROM 
                groups;";

        var groups = (await connection.QueryAsync<GroupDto>(groupSql)).ToList();

        const string membersSql = @"
            SELECT 
                group_id, 
                user_id
            FROM 
                group_members;";

        var members = (await connection.QueryAsync<GroupMemberDto>(membersSql)).ToList();

        await transaction.CommitAsync();

        var result = new List<Group>();
        foreach (var group in groups)
        {
            var mapped = MapDtoToGroup(group, members);
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
        logger.LogInformation("GroupRepository.AddUserToGroupAsync({groupId}, {userId})", groupId.Value, userId.Value);

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

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await connection.ExecuteAsync(insertSql, parameters);

            await connection.CloseAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }

        return await GetByIdAsync(groupId);
    }

    /// <inheritdoc/>
    public async Task<Result<Group>> RemoveUserFromGroupAsync(GroupId groupId, UserId userId)
    {
        logger.LogInformation("GroupRepository.RemoveUserFromGroupAsync({groupId}, {userId})", groupId.Value, userId.Value);

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

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await connection.ExecuteAsync(deleteSql, parameters);

            await connection.CloseAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }

        return await GetByIdAsync(groupId);
    }

    private Result<Group> MapDtoToGroup(GroupDto group, List<GroupMemberDto> members)
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
