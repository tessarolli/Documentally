// <copyright file="IPasswordHasher.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Common.Abstractions;

/// <summary>
/// Interface for the Password Hasher.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// The Password Hashing Implementation.
    /// </summary>
    /// <param name="password">The password string.</param>
    /// <returns>The hashed password string.</returns>
    string HashPassword(string password);
}