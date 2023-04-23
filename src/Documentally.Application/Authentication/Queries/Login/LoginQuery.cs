// <copyright file="LoginQuery.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Authentication.Results;

namespace Documentally.Application.Authentication.Queries.Login;

/// <summary>
/// Contract for the Login Query.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Reviewed")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "Reviewed")]
public record LoginQuery(
    string Email,
    string Password) : IQuery<AuthenticationResult>;
