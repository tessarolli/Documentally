// <copyright file="ExceptionExtensions.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Documentally.Infrastructure.Extensions;

/// <summary>
/// Provides Extensions Methods for Exceptions.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Extension Method to Display a pretty message to the user when an exception occurs.
    /// </summary>
    /// <param name="ex">Exception instance.</param>
    /// <param name="logger">Logger instance.</param>
    /// <returns>A pretty text message.</returns>
    public static string GetPrettyMessage(this Exception ex, ILogger logger)
    {
        string msg;

        if (ex.InnerException != null && ex.InnerException is SocketException)
        {
            msg = "There was a problem connecting to the database.";
        }
        else
        {
            msg = ex switch
            {
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23505" => "The record already exists.",
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23503" => "A required foreign key value does not exist.",
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23514" => "The record violates a check constraint.",
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23502" => "The record violates a not-null constraint.",
                NpgsqlException npgsqlException when npgsqlException.SqlState == "42P01" => "The table does not exist.",
                InvalidOperationException => "The operation is invalid.",
                ArgumentException => "The argument is invalid.",
                TimeoutException => "The operation timed out.",
                _ => "An error occurred while executing a database operation."
            };
        }

        logger.LogError(ex, "{ex}", msg);

        return msg;
    }
}
