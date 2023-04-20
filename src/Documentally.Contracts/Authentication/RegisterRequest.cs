// <copyright file="RegisterRequest.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Contracts.Authentication;

/// <summary>
/// Contract for the Register Request
/// </summary>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
/// <param name="Email"></param>
/// <param name="Password"></param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Contract Parameters should be PascalCase")]
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);
