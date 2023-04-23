﻿// <copyright file="AddUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.User.Results;
using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.UserAggregate.ValueObjects;
using FluentResults;

namespace Documentally.Application.User.Commands.AddUser;

/// <summary>
/// Implementation for the AddUserCommandHandler.
/// </summary>
public class AddUserCommandHandler : ICommandHandler<AddUserCommand, UserResult>
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    /// <param name="passwordHasher">Injected PasswordHasher.</param>
    public AddUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<Result<UserResult>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = Domain.UserAggregate.User.Create(
                   null,
                   request.FirstName,
                   request.LastName,
                   request.Email,
                   request.Password,
                   passwordHasher: passwordHasher);

        if (userResult.IsSuccess)
        {
            var addResult = await userRepository.AddAsync(userResult.Value);
            if (addResult.IsSuccess)
            {
                return Result.Ok(new UserResult(
                    addResult.Value.Id.Value,
                    addResult.Value.FirstName,
                    addResult.Value.LastName,
                    addResult.Value.Email,
                    addResult.Value.Password.Value,
                    (int)addResult.Value.Role,
                    addResult.Value.CreatedAtUtc));
            }
            else
            {
                return Result.Fail(addResult.Errors);
            }
        }
        else
        {
            return Result.Fail(userResult.Errors);
        }
    }
}
