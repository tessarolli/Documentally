// <copyright file="UserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Common.Errors;
using Documentally.Domain.User;
using Documentally.Infrastructure.Abstractions;
using Documentally.Infrastructure.DataTransferObjects;
using FluentResults;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Documentally.Infrastructure.Repository;

/// <summary>
/// The User Repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">IPostgresSqlConnectionFactory to inject.</param>
    /// <param name="logger">ILogger to inject.</param>
    public UserRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory, ILogger<UserRepository> logger)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<User>> AddAsync(User user)
    {
        logger.LogInformation("UserRepository.AddAsync()");

        await using var connection = postgresSqlConnectionFactory.CreateConnection();

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
        catch (PostgresException pex)
        {
            logger.LogError(pex, "Error while inserting user into the database.");

            return Result.Fail("There was a problem when persisting the User to the database.");
        }
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        logger.LogInformation("UserRepository.GetByEmailAsync({email})", email);

        await using var connection = postgresSqlConnectionFactory.CreateConnection();

        var sql = "SELECT * FROM users WHERE email = @email";

        var parameters = new
        {
            email,
        };

        try
        {
            await connection.OpenAsync();

            var userDto = await connection.QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            await connection.CloseAsync();

            return CreateUserResultFromUserDto(userDto);
        }
        catch (PostgresException pex)
        {
            logger.LogError(pex, "Error while retrieving user by Email from the database.");

            return Result.Fail("There was an error while retrieving the User from the Database.");
        }
    }

    /// <inheritdoc/>
    public async Task<Result<User>> GetByIdAsync(long id)
    {
        logger.LogInformation("UserRepository.GetByIdAsync({id})", id);

        await using var connection = postgresSqlConnectionFactory.CreateConnection();

        var sql = "SELECT * FROM users WHERE id = @id";

        var parameters = new
        {
            id,
        };

        try
        {
            await connection.OpenAsync();

            var userDto = await connection.QueryFirstOrDefaultAsync<UserDto>(sql, parameters);

            await connection.CloseAsync();

            return CreateUserResultFromUserDto(userDto);
        }
        catch (PostgresException pex)
        {
            logger.LogError(pex, "Error while retrieving user by Email from the database.");

            return Result.Fail("There was an error while retrieving the User from the Database.");
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
