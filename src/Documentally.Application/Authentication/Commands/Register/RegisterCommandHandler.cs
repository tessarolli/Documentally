// <copyright file="RegisterCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Authentication.Common;
using Documentally.Application.Authentication.Errors;
using Documentally.Domain.Entities;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Commands.Register;

/// <summary>
/// The implementation for the Register Command.
/// </summary>
public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">UserRepository being injected.</param>
    /// <param name="jwtTokenGenerator">JwtTokenGenerator being injected.</param>
    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.userRepository = userRepository;
    }

    /// <summary>
    /// The actual command Handler.
    /// </summary>
    /// <param name="command">RegisterCommand.</param>
    /// <param name="cancellationToken">Async CancellationToken.</param>
    /// <returns>FluentResult for the operation.</returns>
    public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Check if User with given e-mail already exists
        if (await userRepository.GetByEmailAsync(command.Email) is not null)
        {
            return Result.Fail(new UserWithEmailAlreadyExistsError());
        }

        // Create a new user
        var userResult = User.Create(command.FirstName, command.LastName, command.Email, command.Password);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        // Add to the Database
        var persistResult = await userRepository.AddAsync(userResult.Value);
        if (persistResult.IsFailed)
        {
            return Result.Fail(persistResult.Errors);
        }

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(userResult.Value);

        return new AuthenticationResult(userResult.Value, token);
    }
}