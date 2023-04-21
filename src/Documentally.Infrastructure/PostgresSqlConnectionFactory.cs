// <copyright file="PostgresSqlConnectionFactory.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Documentally.Infrastructure;

/// <summary>
/// Postgres Sql Connection Factory.
/// </summary>
public class PostgresSqlConnectionFactory : IPostgresSqlConnectionFactory
{
    /// <summary>
    /// Holds the Connection String Configuration name on the appsettings.json file on the API project.
    /// </summary>
    public static readonly string ConnectionStringConfigurationName = "PostgresSqlConnectionString";

    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresSqlConnectionFactory"/> class.
    /// </summary>
    /// <param name="configuration">Injected configuration.</param>
    public PostgresSqlConnectionFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <inheritdoc/>
    public NpgsqlConnection CreateConnection()
    {
        var connectionString = configuration[ConnectionStringConfigurationName];
        return new NpgsqlConnection(connectionString);
    }
}
