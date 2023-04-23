// <copyright file="DeleteUserCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.UserAggregate.ValueObjects;
using FluentResults;

namespace Documentally.Application.User.Commands.DeleteUser;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.RemoveAsync(new UserId(request.Id));
    }
}
