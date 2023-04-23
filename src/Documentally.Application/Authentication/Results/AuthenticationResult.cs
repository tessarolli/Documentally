// <copyright file="AuthenticationResult.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.UserAggregate;

namespace Documentally.Application.Authentication.Results;

/// <summary>
/// Contract for an Authentication Result response.
/// </summary>
/// <param name="User"></param>
/// <param name="Token"></param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Reviewed")]
public record AuthenticationResult(
    Domain.UserAggregate.User User,
    string Token);
