// <copyright file="UserRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Dapper;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Entities;
using Documentally.Infrastructure.Abstractions;
using FluentResults;
using Npgsql;

namespace Documentally.Infrastructure.Repository;

/// <summary>
/// The User Repository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">to inject.</param>
    public UserRepository(IPostgresSqlConnectionFactory postgresSqlConnectionFactory)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
    }

    /// <inheritdoc/>
    public async Task<Result<User>> AddAsync(User user)
    {
        await using var connection = postgresSqlConnectionFactory.CreateConnection();

        var sql = "INSERT INTO \"Users\" (" +
                      "\"FirstName\"," +
                      "\"LastName\"," +
                      "\"Email\"," +
                      "\"Password\"," +
                      "\"Role\") " +
                  "VALUES (" +
                      "@FirstName," +
                      "@LastName," +
                      "@Email," +
                      "@Password," +
                      "@Role) " +
                  "RETURNING " +
                      "\"Id\"";

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
        }
        catch (PostgresException pex)
        {
            // LogException(pex);
            return Result.Fail("User database insertion failed");
        }

        // user.Id = new UserId(newId);
        return Result.Ok(user);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        await using var connection = postgresSqlConnectionFactory.CreateConnection();

        var sql = "SELECT * from \"Users\" where \"Email\" = @Email";

        var parameters = new
        {
            Email = email,
        };

        try
        {
            await connection.OpenAsync();

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);

            await connection.CloseAsync();

            return user;
        }
        catch (PostgresException pex)
        {
            // LogException(pex);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(long id)
    {
        await using var connection = postgresSqlConnectionFactory.CreateConnection();

        var sql = "SELECT * from \"Users\" where \"Id\" = @Id";

        var parameters = new
        {
            Id = id,
        };

        try
        {
            await connection.OpenAsync();

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);

            await connection.CloseAsync();

            return user;
        }
        catch (PostgresException pex)
        {
            // LogException(pex);
            return null;
        }
    }
}
