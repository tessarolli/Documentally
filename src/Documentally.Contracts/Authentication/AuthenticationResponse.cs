// <copyright file="AuthenticationResponse.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Authentication;

/// <summary>
/// Contract for an Authentication Request's Response
/// </summary>
/// <param name="Id"></param>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
/// <param name="Email"></param>
/// <param name="Token"></param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Contracts should be PascalCase")]
public record AuthenticationResponse(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);
