// <copyright file="AuthenticatedUserService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Abstractions.Services;
using Documentally.Domain.User;
using Microsoft.AspNetCore.Http;

namespace Documentally.Infrastructure.Services;

/// <summary>
/// Get the Authenticated User Entity from the Json Web Token.
/// </summary>
public class AuthenticatedUserService : IAuthenticatedUserService
{
    private readonly IUserRepository userRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedUserService"/> class.
    /// </summary>
    /// <param name="userRepository">Inject User Repository.</param>
    /// <param name="httpContextAccessor">Inject HttpContextAccessor.</param>
    public AuthenticatedUserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        this.userRepository = userRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public async Task<User?> GetAuthenticatedUserAsync()
    {
        var userId = httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;

        if (userId is not null)
        {
            var userResult = await userRepository.GetByIdAsync(long.Parse(userId));
            if (userResult.IsSuccess)
            {
                return userResult.Value;
            }

            return null;
        }

        throw new UnauthorizedAccessException("User not authenticated.");
    }
}