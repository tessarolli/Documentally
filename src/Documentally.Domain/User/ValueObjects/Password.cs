// <copyright file="Password.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using FluentResults;

namespace Documentally.Domain.User.ValueObjects;

/// <summary>
/// Password Value Object.
/// </summary>
public class Password : ValueObject
{
    private const int MinPasswordLength = 3;

    /// <summary>
    /// Initializes a new instance of the <see cref="Password"/> class.
    /// </summary>
    /// <param name="value">The password value.</param>
    private Password(string value) => Value = value;

    /// <summary>
    /// Gets the value of this password.
    /// </summary>
    public string Value { get; } = string.Empty;

    /// <summary>
    /// Creates a new <see cref="Password"/> instance.
    /// </summary>
    /// <param name="password">The password string.</param>
    /// <returns>The password instance, or an error.</returns>
    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return Result.Fail("Password is required");
        }

        if (password.Length < MinPasswordLength)
        {
            return Result.Fail($"Password should be at least {MinPasswordLength} characters");
        }

        return new Password(password);
    }

    /// <summary>
    /// Get the equality components.
    /// </summary>
    /// <returns>The properties enumeration.</returns>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
