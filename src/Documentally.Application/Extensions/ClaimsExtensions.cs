// <copyright file="ClaimsExtensions.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using System.Security.Claims;

namespace Documentally.Infrastructure.Extensions;

/// <summary>
/// Claims Extensions.
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    /// Extracts the Json Token Identifier from the Claims.
    /// </summary>
    /// <param name="user">ClaimsPrincipal instance.</param>
    /// <returns>The Jti value or null.</returns>
    public static string? GetJti(this ClaimsPrincipal user)
    {
        return user?.FindFirstValue("jti");
    }

    /// <summary>
    /// Extracts the Json Token Identifier from the Claims.
    /// </summary>
    /// <param name="user">ClaimsPrincipal instance.</param>
    /// <returns>The Jti value or null.</returns>
    public static long? GetJtiAsLong(this ClaimsPrincipal user)
    {
        var value = user?.FindFirstValue("jti");
        if (long.TryParse(value, out var longValue))
        {
            return longValue;
        }

        return null;
    }
}