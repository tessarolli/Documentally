// <copyright file="JwtSettings.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Infrastructure.Authentication;

/// <summary>
/// Helper class to setup configurations options for Json Web Tokens.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets The section name for the appsettings.json.
    /// </summary>
    public static string SectionName { get; } = "JwtSettings";

    /// <summary>
    /// Gets the Secret for cryptographing the token.
    /// </summary>
    public string Secret { get; init; } = null!;

    /// <summary>
    /// Gets the Expire Days for the token.
    /// </summary>
    public int ExpireDays { get; init; }

    /// <summary>
    /// Gets the Issuer of the token.
    /// </summary>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// Gets the Audience of the token.
    /// </summary>
    public string Audience { get; init; } = null!;
}
