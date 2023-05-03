// <copyright file="IPasswordHashingService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Common.Abstractions;

/// <summary>
/// Interface for the Password Hasher.
/// </summary>
public interface IPasswordHashingService
{
    /// <summary>
    /// The Password Hashing Implementation.
    /// </summary>
    /// <param name="password">The password string.</param>
    /// <param name="iterations">The number of iterations.</param>
    /// <returns>The hashed password string.</returns>
    string HashPassword(string password, int iterations = 10000);

    /// <summary>
    /// Verifies if the informed password is valid.
    /// </summary>
    /// <param name="passwordToVerify">The plain text password string.</param>
    /// <param name="hashedPassword">The hashed password string.</param>
    /// <returns>True if they match.</returns>
    bool VerifyPassword(string passwordToVerify, string hashedPassword);
}