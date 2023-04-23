// <copyright file="UpdateUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.User.Results;
using Documentally.Domain.Common.Abstractions;
using FluentResults;

namespace Documentally.Application.User.Commands.UpdateUser;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResult>
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected userRepository.</param>
    /// <param name="passwordHasher">Injected passwordHasher.</param>
    public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<Result<UserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = Domain.UserAggregate.User.Create(
            request.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            passwordHasher: passwordHasher);

        if (userResult.IsSuccess)
        {
            var updateResult = await userRepository.UpdateAsync(userResult.Value);
            if (updateResult.IsSuccess)
            {
                return Result.Ok(new UserResult(
                    updateResult.Value.Id.Value,
                    updateResult.Value.FirstName,
                    updateResult.Value.LastName,
                    updateResult.Value.Email,
                    updateResult.Value.Password.Value,
                    (int)updateResult.Value.Role,
                    updateResult.Value.CreatedAtUtc));
            }
            else
            {
                return Result.Fail(updateResult.Errors);
            }
        }
        else
        {
            return Result.Fail(userResult.Errors);
        }
    }
}
