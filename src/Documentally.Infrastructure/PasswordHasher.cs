// <copyright file="PasswordHasher.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using Documentally.Domain.Common.Abstractions;

namespace Documentally.Infrastructure;

/// <summary>
/// Password hashing.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;

    /// <inheritdoc/>
    public string HashPassword(string password)
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var saltString = Convert.ToBase64String(salt);

        return $"{Iterations}.{saltString}.{key}";
    }
}
