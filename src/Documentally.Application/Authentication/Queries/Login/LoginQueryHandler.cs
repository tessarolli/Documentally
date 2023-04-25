// <copyright file="LoginQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Authentication.Errors;
using Documentally.Application.Authentication.Results;
using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.User;
using FluentResults;

namespace Documentally.Application.Authentication.Queries.Login;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly IUserRepository userRepository;
    private readonly IPasswordHashingService passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    /// <param name="jwtTokenGenerator">Injected JwtTokenGenerator.</param>
    /// <param name="passwordHasher">Injected PasswordHasher.</param>
    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHashingService passwordHasher)
    {
        this.userRepository = userRepository;
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<Result<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // Check if User with given e-mail already exists
        Result<User> userResult = await userRepository.GetByEmailAsync(query.Email);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        // Validate the Password
        if (!passwordHasher.VerifyPassword(query.Password, userResult.Value.Password.Value))
        {
            return Result.Fail(new InvalidPasswordError());
        }

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(userResult.Value);

        return new AuthenticationResult(userResult.Value, token);
    }
}