// <copyright file="DapperUtility.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Data;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Documentally.Infrastructure.Abstractions;
using Npgsql;

namespace Documentally.Infrastructure.Utilities;

/// <summary>
/// Provides Dapper Accessibility Helpers Functunalities.
/// </summary>
internal sealed class DapperUtility
{
    private readonly IPostgresSqlConnectionFactory postgresSqlConnectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DapperUtility"/> class.
    /// </summary>
    /// <param name="postgresSqlConnectionFactory">PostgresConnectionFactory injected.</param>
    public DapperUtility(IPostgresSqlConnectionFactory postgresSqlConnectionFactory)
    {
        this.postgresSqlConnectionFactory = postgresSqlConnectionFactory;
    }

    /// <summary>
    /// Query the database.
    /// </summary>
    /// <typeparam name="T">The type to map the result.</typeparam>
    /// <param name="sql">sql or stored procedure to call.</param>
    /// <param name="parameters">parmeters.</param>
    /// <param name="commandType">type of command.</param>
    /// <param name="transaction">Transcation.</param>
    /// <returns>A IEnumerable of TResult.</returns>
    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.Text,
        NpgsqlTransaction? transaction = null)
    {
        using var connection = postgresSqlConnectionFactory.CreateConnection();

        if (transaction is null)
        {
            await connection.OpenAsync();
        }

        var result = await connection.QueryAsync<T>(sql, parameters, commandType: commandType, transaction: transaction);

        if (transaction is null)
        {
            await connection.CloseAsync();
        }

        return result;
    }

    /// <summary>
    /// Query the First result from the database or return null if it doesnt exists.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sql">sql or stored procedure to call.</param>
    /// <param name="parameters">parmeters.</param>
    /// <param name="commandType">type of command.</param>
    /// <param name="transaction">Transaction.</param>
    /// <returns>A TResult instance or null.</returns>
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
      string sql,
      object? parameters = null,
      CommandType commandType = CommandType.Text,
      NpgsqlTransaction? transaction = null)
    {
        using var connection = postgresSqlConnectionFactory.CreateConnection();

        if (transaction is null)
        {
            await connection.OpenAsync();
        }

        var result = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType, transaction: transaction);

        if (transaction is null)
        {
            await connection.CloseAsync();
        }

        return result;
    }

    /// <summary>
    /// Executes an stored procedure or an sql statement.
    /// </summary>
    /// <param name="sql">sql statement or store procedure name.</param>
    /// <param name="parameters">parameters.</param>
    /// <param name="commandType">command type.</param>
    /// <param name="transaction">Transaction.</param>
    /// <returns>Number of rows affected.</returns>
    public async Task<long> ExecuteAsync(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.Text,
        NpgsqlTransaction? transaction = null)
    {
        using var connection = transaction?.Connection ?? postgresSqlConnectionFactory.CreateConnection();

        if (transaction is null)
        {
            await connection.OpenAsync();
        }

        var rowsAffectedCount = await connection.ExecuteAsync(sql, parameters, commandType: commandType, transaction: transaction);

        if (transaction is null)
        {
            await connection.CloseAsync();
        }

        return rowsAffectedCount;
    }

    /// <summary>
    /// Executes an stored procedure or an sql statement.
    /// </summary>
    /// <param name="sql">sql statement or store procedure name.</param>
    /// <param name="parameters">parameters.</param>
    /// <param name="commandType">command type.</param>
    /// <param name="transaction">Transaction.</param>
    /// <returns>An scalar number from the executed command.</returns>
    public async Task<long> ExecuteScalarAsync(
        string sql,
        object? parameters = null,
        CommandType commandType = CommandType.Text,
        NpgsqlTransaction? transaction = null)
    {
        using var connection = postgresSqlConnectionFactory.CreateConnection();

        if (transaction is null)
        {
            await connection.OpenAsync();
        }

        var scalar = await connection.ExecuteScalarAsync<long>(sql, parameters, commandType: commandType, transaction: transaction);

        if (transaction is null)
        {
            await connection.CloseAsync();
        }

        return scalar;
    }

    /// <summary>
    /// Opens a transaction.
    /// </summary>
    /// <returns>The Transaction.</returns>
    public async Task<NpgsqlTransaction> BeginTransactionAsync()
    {
        using var connection = postgresSqlConnectionFactory.CreateConnection();

        await connection.OpenAsync();

        return await connection.BeginTransactionAsync();
    }
}
