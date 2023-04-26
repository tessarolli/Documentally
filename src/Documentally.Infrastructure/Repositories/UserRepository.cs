// <copyright file="UserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Data;
using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Common.Errors;
using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.User;
using Documentally.Domain.User.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Utilities;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The User Repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;
    private readonly ILogger logger;
    private readonly IPasswordHashingService passwordHasher;
    private readonly DapperUtility db;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="passwordHasher">IPasswordHashingService to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    public UserRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<UserRepository> logger, IPasswordHashingService passwordHasher)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
        this.logger = logger;
        this.passwordHasher = passwordHasher;
        this.db = new DapperUtility(postgresSqlConnectionFactory);
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByIdAsync(long id)
    {
        var sql = "SELECT * FROM users WHERE id = @id";

        var userDto = await db.QueryFirstOrDefaultAsync<UserDto>(sql, new { id }, CommandType.Text);

        return CreateUserResultFromUserDto(userDto);
    }

    /// <inheritdoc/>
    public async Task<List<User>> GetByIdsAsync(long[] ids)
    {
        var sql = "SELECT * FROM users WHERE id = ANY(@ids)";

        var parameters = new
        {
            ids,
        };

        var userDtos = await db.QueryAsync<UserDto>(sql, new { ids }, CommandType.Text);

        return userDtos
            .Select(CreateUserResultFromUserDto)
            .Where(x => x.IsSuccess)
            .Select(x => x.Value)
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<Result<List<User>>> GetAllAsync()
    {
        var sql = "SELECT * FROM users";

        var userDtos = await db.QueryAsync<UserDto>(sql, null, CommandType.Text);

        return userDtos
            .Select(CreateUserResultFromUserDto)
            .Where(x => x.IsSuccess)
            .Select(x => x.Value)
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        logger.LogInformation("UserRepository.GetByEmailAsync({email})", email);

        var sql = "SELECT * FROM users WHERE email = @email";

        var userDto = await db.QueryFirstOrDefaultAsync<UserDto>(sql, new { email }, CommandType.Text);

        return CreateUserResultFromUserDto(userDto);
    }

    /// <inheritdoc/>
    public async Task<Result<User>> AddAsync(User user)
    {
        if (!user.Password.IsHashed)
        {
            user.Password.HashPassword(passwordHasher);
        }

        var sql = "INSERT INTO users (" +
                      "first_name," +
                      "last_name," +
                      "email," +
                      "password," +
                      "role) " +
                  "VALUES (" +
                      "@FirstName," +
                      "@LastName," +
                      "@Email," +
                      "@Password," +
                      "@Role) " +
                  "RETURNING " +
                      "id";

        var parameters = new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Password = user.Password.HashedPassword,
            Role = (int)user.Role,
        };

        var newId = await db.ExecuteScalarAsync(sql, parameters, CommandType.Text);

        var newUserResult = User.Create(
                newId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password.HashedPassword,
                user.Role,
                user.CreatedAtUtc);

        if (newUserResult.IsFailed)
        {
            var errMsg = "The new user was persisted on the database, but the entity creation failed somehow.";

            logger.LogError("{errMsg}", errMsg);

            return Result.Fail(errMsg);
        }

        return Result.Ok(newUserResult.Value);
    }

    /// <inheritdoc/>
    public async Task<Result<User>> UpdateAsync(User user)
    {
        if (!user.Password.IsHashed)
        {
            user.Password.HashPassword(passwordHasher);
        }

        var parameters = new
        {
            p_id = user.Id.Value,
            p_first_name = user.FirstName,
            p_last_name = user.LastName,
            p_email = user.Email,
            p_password = user.Password.HashedPassword,
            p_role = user.Role,
        };

        var n = await db.ExecuteAsync("sp_update_user", parameters, CommandType.StoredProcedure);

        return Result.Ok(user);
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(UserId userId)
    {
        var parameters = new
        {
            removing_user_id = userId.Value,
        };

        await db.ExecuteAsync("remove_user", parameters, CommandType.StoredProcedure);

        return Result.Ok();
    }

    /// <summary>
    /// Creates a new instance of the User Entity, using the Create factory method.
    /// </summary>
    /// <param name="userDto">The User Data Transfer Object.</param>
    /// <returns>An Result indicating the status of the operation.</returns>
    private static Result<User> CreateUserResultFromUserDto(UserDto? userDto)
    {
        if (userDto is not null)
        {
            var userResult = User.Create(
                userDto.id,
                userDto.first_name,
                userDto.last_name,
                userDto.email,
                userDto.password,
                userDto.role);
            if (userResult.IsSuccess)
            {
                return Result.Ok(userResult.Value);
            }

            return Result.Fail(userResult.Errors);
        }

        return Result.Fail(new NotFoundError($"User not found."));
    }
}
