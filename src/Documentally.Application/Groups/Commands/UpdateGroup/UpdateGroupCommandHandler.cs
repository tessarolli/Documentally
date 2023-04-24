// <copyright file="UpdateGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Groups.Results;
using Documentally.Domain.Group.ValueObjects;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Groups.Commands.UpdateGroup;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class UpdateGroupCommandHandler : ICommandHandler<UpdateGroupCommand, GroupResult>
{
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    public UpdateGroupCommandHandler(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<GroupResult>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var groupResult = Domain.Group.Group.Create(
            new GroupId(request.Id),
            request.Name,
            new UserId(request.OwnerId));

        if (groupResult.IsSuccess)
        {
            var updateResult = await groupRepository.UpdateAsync(groupResult.Value);
            if (updateResult.IsSuccess)
            {
                return Result.Ok(new GroupResult(
                    groupResult.Value.Id.Value,
                    groupResult.Value.Name,
                    groupResult.Value.OwnerId.Value,
                    groupResult.Value.MemberIds.Select(x => x.Value).ToList(),
                    groupResult.Value.CreatedAtUtc));
            }
            else
            {
                return Result.Fail(updateResult.Errors);
            }
        }
        else
        {
            return Result.Fail(groupResult.Errors);
        }
    }
}
