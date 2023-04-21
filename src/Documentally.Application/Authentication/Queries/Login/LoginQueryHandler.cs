// <copyright file="LoginQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Authentication.Common;
using Documentally.Application.Authentication.Errors;
using Documentally.Domain.Entities;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Queries.Login;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    /// <param name="jwtTokenGenerator">Injected JwtTokenGenerator.</param>
    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        this.userRepository = userRepository;
        this.jwtTokenGenerator = jwtTokenGenerator;
    }

    /// <inheritdoc/>
    public async Task<Result<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // Check if User with given e-mail already exists
        if (await userRepository.GetByEmailAsync(query.Email) is not User user)
        {
            return Result.Fail(new UserWithEmailNotFoundError());
        }

        // Validate the Password
        if (user.Password.Value != query.Password)
        {
            return Result.Fail(new InvalidPasswordError());
        }

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}