// <copyright file="IJwtTokenGenerator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Entities;

namespace Documentally.Application.Interfaces.Infrastructure;

/// <summary>
/// Interface for Json Web Tokens Generator.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JWT token.
    /// </summary>
    /// <param name="user">The User to generate the token for.</param>
    /// <returns>The token.</returns>
    string GenerateToken(User user);
}
