// <copyright file="GetUsersListQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.User.Results;
using FluentResults;

namespace Documentally.Application.User.Queries.GetUsersList;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class GetUsersListQueryHandler : IQueryHandler<GetUsersListQuery, List<UserResult>>
{
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersListQueryHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Injected UserRepository.</param>
    public GetUsersListQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<List<UserResult>>> Handle(GetUsersListQuery query, CancellationToken cancellationToken)
    {
        var result = new List<UserResult>();

        var usersResult = await userRepository.GetAllAsync();
        if (usersResult.IsSuccess)
        {
            foreach (var userResult in usersResult.Value)
            {
                result.Add(new UserResult(
                    userResult.Id.Value,
                    userResult.FirstName,
                    userResult.LastName,
                    userResult.Email,
                    userResult.Password.Value,
                    (int)userResult.Role,
                    userResult.CreatedAtUtc));
            }
        }

        return Result.Ok(result);
    }
}