// <copyright file="IAuthenticatedUserService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.UserAggregate;

namespace Documentally.Application.Authentication.Services;

/// <summary>
/// Provides funcionality for Accessing the Authenticated User Entity.
/// </summary>
public interface IAuthenticatedUserService
{
    /// <summary>
    /// Gets the Authenticated User Entity from the Json Web Token.
    /// </summary>
    /// <returns>The Authenticated User Entity instance.</returns>
    Task<Domain.UserAggregate.User?> GetAuthenticatedUserAsync();
}