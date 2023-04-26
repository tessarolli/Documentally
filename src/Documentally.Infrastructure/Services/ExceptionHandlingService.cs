// <copyright file="ExceptionHandlingService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Net.Sockets;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Common.Errors;
using FluentResults;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Documentally.Infrastructure.Services;

/// <summary>
/// Exception Handling Service.
/// </summary>
public class ExceptionHandlingService : IExceptionHandlingService
{
    /// <inheritdoc/>
    public Result HandleException(Exception exception, ILogger? logger = null)
    {
        Error msg;

        if (exception.InnerException != null && exception.InnerException is SocketException)
        {
            msg = new Error("A network connectivity problem occurred while trying to contact an external service required to handle this request.");
        }
        else
        {
            msg = exception switch
            {
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23505" => new ConflictError("A record with the same key already exists."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23503" => new NotFoundError("The required relation does not exist."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23514" => new Error("The record violates a check constraint."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "23502" => new Error("The record violates a not-null constraint."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "42P01" => new NotFoundError("The table does not exist."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "2201W" => new Error("A value in the record violates the length constraint."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "22003" => new Error("A value in the record violates the numeric constraint."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "22P02" => new Error("A value in the record violates the data type constraint."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "22P05" => new Error("The JSON value in the record is not valid."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "22P06" => new Error("The array value in the record is not valid."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "42703" => new NotFoundError("A column in the record does not exist."),
                NpgsqlException npgsqlException when npgsqlException.SqlState == "42601" => new Error("The query is not valid."),
                InvalidOperationException => new Error("The operation is invalid."),
                ArgumentException => new Error("The argument is invalid."),
                TimeoutException => new Error("The operation timed out."),
                KeyNotFoundException => new NotFoundError("The provided key was not found."),
                _ => new Error("An unexpected error occurred on our end. We are looking into it. In the mean time, try the request again.")
            };
        }

        logger?.LogError(exception, "{ex}", msg);

        return Result.Fail(msg);
    }
}
