// <copyright file="GetUserByIdQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Users.Results;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Users.Queries.GetUserById;

/// <summary>
/// Implementation for the GetUserByIdQuery.
/// </summary>
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResult>
{
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<UserResult>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var userResult = await userRepository.GetByIdAsync(query.Id);
        if (userResult.IsSuccess)
        {
            return Result.Ok(new UserResult(
                userResult.Value.Id.Value,
                userResult.Value.FirstName,
                userResult.Value.LastName,
                userResult.Value.Email,
                userResult.Value.Password.Value,
                (int)userResult.Value.Role,
                userResult.Value.CreatedAtUtc));
        }

        return Result.Fail(userResult.Errors);
    }
}