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

            // Add a group to the domain group aggregate.
            var addMemberToGroupResult = group.AddMember(new UserId(request.UserId));
            if (addMemberToGroupResult.IsFailed)
            {
                return new ConflictError(addMemberToGroupResult.Errors.First().Message);
            }

            // Update the Group Aggregate in the repository.
            var updateGroupResult = await groupRepository.UpdateAsync(group);
            if (updateGroupResult.IsSuccess)
            {
                return Result.Ok(new GroupResult(
                    updateGroupResult.Value.Id.Value,
                    updateGroupResult.Value.Name,
                    updateGroupResult.Value.OwnerId.Value,
                    updateGroupResult.Value.MemberIds.Select(x => x.Value).ToList(),
                    updateGroupResult.Value.CreatedAtUtc));
            }
            else
            {
                return Result.Fail(updateGroupResult.Errors);
            }
        }
        else
        {
            return Result.Fail(groupResult.Errors);
        }
    }
}
