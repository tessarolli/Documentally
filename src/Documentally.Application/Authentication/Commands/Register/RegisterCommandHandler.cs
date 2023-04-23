// <copyright file="RegisterCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Authentication;
using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Authentication.Errors;
using Documentally.Application.Authentication.Results;
using Documentally.Application.Common.Errors;
using Documentally.Application.EventBus;
using Documentally.Domain.UserAggregate;
using Documentally.Domain.UserAggregate.Events;
using FluentResults;

namespace Documentally.Application.Authentication.Commands.Register;

/// <summary>
/// The implementation for the Register Command.
/// </summary>
public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly IUserRepository userRepository;
    private readonly IDomainEventBus domainEventBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">IUserRepository being injected.</param>
    /// <param name="jwtTokenGenerator">IJwtTokenGenerator being injected.</param>
    /// <param name="domainEventBus">IDomainEventBus being injected.</param>
    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IDomainEventBus domainEventBus)
    {
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.userRepository = userRepository;
        this.domainEventBus = domainEventBus;
    }

    /// <summary>
    /// The actual command Handler implementation for registering a new user.
    /// </summary>
    /// <param name="command">RegisterCommand.</param>
    /// <param name="cancellationToken">Async CancellationToken.</param>
    /// <returns>FluentResult for the operation.</returns>
    public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Check if a user with the given e-mail already exists
        // Although this is a business rule, it's more of an application complexity than a domain complexity
        // Therefore, we can handle this rule enforcement here in the application layer.
        var getUserByEmailResult = await userRepository.GetByEmailAsync(command.Email);
        if (getUserByEmailResult.IsFailed)
        {
            if (!getUserByEmailResult.HasError<NotFoundError>())
            {
                return Result.Fail(getUserByEmailResult.Errors);
            }
        }
        else
        {
            return Result.Fail(new UserWithEmailAlreadyExistsError());
        }

        // Now that we Ensure the Business Rule, we can carry on with the User Creation
        // and persisting it to the repository.
        var userResult = Domain.UserAggregate.User.Create(null, command.FirstName, command.LastName, command.Email, command.Password);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        // Persist newly created User Entity Instance to the Repository
        var persistResult = await userRepository.AddAsync(userResult.Value);
        if (persistResult.IsFailed)
        {
            return Result.Fail(persistResult.Errors);
        }

        domainEventBus.DispatchDomainEvent(new UserCreatedDomainEvent(persistResult.Value));

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(persistResult.Value);

        return new AuthenticationResult(persistResult.Value, token);
    }
}