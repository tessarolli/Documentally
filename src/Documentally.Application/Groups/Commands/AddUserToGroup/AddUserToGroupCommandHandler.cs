// <copyright file="AddUserToGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Common.Errors;
using Documentally.Application.Groups.Results;
using Documentally.Domain.Enums;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Groups.Commands.AddUserToGroup;

/// <summary>
/// Implementation for the AddUserToGroupCommandHandler.
/// </summary>
public class AddUserToGroupCommandHandler : ICommandHandler<AddUserToGroupCommand, GroupResult>
{
    private readonly IGroupRepository groupRepository;
    private readonly IAuthenticatedUserService authenticatedUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserToGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    /// <param name="authenticatedUserService">Injected AuthenticatedUserService.</param>
    public AddUserToGroupCommandHandler(IGroupRepository groupRepository, IAuthenticatedUserService authenticatedUserService)
    {
        this.groupRepository = groupRepository;
        this.authenticatedUserService = authenticatedUserService;
    }

    /// <inheritdoc/>
    public async Task<Result<GroupResult>> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
    {
        // Gets the Authenticated User Instance.
        var authenticatedUser = await authenticatedUserService.GetAuthenticatedUserAsync();
        if (authenticatedUser is null)
        {
            return Result.Fail("Could not restore the Authenticated User.");
        }

        // Get the Group Aggregate from the repository
        var groupResult = await groupRepository.GetByIdAsync(new GroupId(request.GroupId));
        if (groupResult.IsSuccess)
        {
            var group = groupResult.Value;

            // Authenticated User is:
            //      Not the Owner of this group
            //      Not an Manager or an Admin
            if (group.OwnerId.Value != authenticatedUser.Id.Value &&
                authenticatedUser.Role == Roles.User)
            {
                // Then he is not authorized to perform this action
                return Result.Fail(new UnauthorizedError("Only the Group Creator or Admins/Managers can perform this action!"));
            }

            // If we got here, it means that the authenticated user is allowed to access this resource.
            var addResult = await groupRepository.AddUserToGroupAsync(new GroupId(request.GroupId), new UserId(request.UserId));
            if (addResult.IsSuccess)
            {
                return Result.Ok(new GroupResult(
                    addResult.Value.Id.Value,
                    addResult.Value.Name,
                    addResult.Value.OwnerId.Value,
                    addResult.Value.MemberIds.Select(x => x.Value).ToList(),
                    addResult.Value.CreatedAtUtc));
            }
            else
            {
                return Result.Fail(addResult.Errors);
            }
        }
        else
        {
            return Result.Fail(groupResult.Errors);
        }
    }
}
