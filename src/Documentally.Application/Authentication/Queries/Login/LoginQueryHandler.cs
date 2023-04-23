// <copyright file="LoginQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Authentication.Errors;
using Documentally.Application.Authentication.Results;
using Documentally.Domain.UserAggregate;
using FluentResults;

namespace Documentally.Application.Authentication.Queries.Login;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
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
        Result<Domain.UserAggregate.User> userResult = await userRepository.GetByEmailAsync(query.Email);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        // Validate the Password
        if (userResult.Value.Password.Value != query.Password)
        {
            return Result.Fail(new InvalidPasswordError());
        }

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(userResult.Value);

        return new AuthenticationResult(userResult.Value, token);
    }
}