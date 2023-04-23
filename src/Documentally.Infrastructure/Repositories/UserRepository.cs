// <copyright file="UserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Common.Errors;
using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.UserAggregate;
using Documentally.Domain.UserAggregate.ValueObjects;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using Documentally.Infrastructure.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Documentally.Infrastructure.Repositories;

/// <summary>
/// The User Repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;
    private readonly ILogger logger;
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="passwordHasher">IPasswordHasher to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    public UserRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<UserRepository> logger, IPasswordHasher passwordHasher)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
        this.logger = logger;
        this.passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByIdAsync(long id)
    {
        logger.LogInformation("UserRepository.GetByIdAsync({id})", id);

        var sql = "SELECT * FROM users WHERE id = @id";

        var parameters = new
        {
            id,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var userDto = await connection.QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            await connection.CloseAsync();

            return CreateUserResultFromUserDto(userDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<List<User>> GetByIdsAsync(long[] ids)
    {
        logger.LogInformation("UserRepository.GetByIdsAsync({ids})", ids);

        var sql = "SELECT * FROM users WHERE id = ANY(@ids)";

        var parameters = new
        {
            ids,
        };

        try
        {
            using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var userDtos = await connection.QueryAsync<UserDto>(sql, parameters);

            await connection.CloseAsync();

            var users = userDtos
                .Select(CreateUserResultFromUserDto)
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList();

            return users;
        }
        catch (Exception ex)
        {
            ex.GetPrettyMessage(logger);
        }

        return new List<User>();
    }

    /// <inheritdoc/>
    public async Task<Result<List<User>>> GetAllAsync()
    {
        logger.LogInformation("UserRepository.GetAllAsync()");

        var sql = "SELECT * FROM users";

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var userDtos = await connection.QueryAsync<UserDto>(sql);

            await connection.CloseAsync();

            var users = userDtos
                .Select(CreateUserResultFromUserDto)
                .Where(x => x.IsSuccess)
                .Select(x => x.Value)
                .ToList();

            return Result.Ok(users);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        logger.LogInformation("UserRepository.GetByEmailAsync({email})", email);

        var sql = "SELECT * FROM users WHERE email = @email";

        var parameters = new
        {
            email,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var userDto = await connection.QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            await connection.CloseAsync();

            return CreateUserResultFromUserDto(userDto);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<User>> AddAsync(User user)
    {
        logger.LogInformation("UserRepository.AddAsync()");

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
            Password = user.Password.Value,
            Role = (int)user.Role,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            var newId = await connection.ExecuteScalarAsync<int>(sql, parameters);

            await connection.CloseAsync();

            var newUserResult = User.Create(
                newId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password.Value,
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
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result<User>> UpdateAsync(User user)
    {
        logger.LogInformation("UserRepository.UpdateAsync({user})", user);

        if (!user.Password.IsHashed)
        {
            user.Password.HashPassword(passwordHasher);
        }

        var sql = @"
            UPDATE 
                users 
            SET
                first_name = @FirsName,
                last_name = @LastName,
                email = @Email,
                password = @Password,
                role = @Role
            WHERE 
                id = @Id";

        var parameters = new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Password,
            user.Role,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            return Result.Ok(user);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
    }

    /// <inheritdoc/>
    public async Task<Result> RemoveAsync(UserId userId)
    {
        logger.LogInformation("UserRepository.RemoveAsync({userId})", userId);

        var sql = "DELETE FROM users WHERE id = @id";

        var parameters = new
        {
            id = userId.Value,
        };

        try
        {
            await using var connection = postgresSqlConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, parameters);

            await connection.CloseAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.GetPrettyMessage(logger));
        }
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

        return Result.Fail(new NotFoundError($"User with the provided E-Mail was not found."));
    }
}
