// <copyright file="AddGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Groups.Results;
using Documentally.Domain.User.ValueObjects;
using FluentResults;

namespace Documentally.Application.Groups.Commands.AddGroup;

/// <summary>
/// Implementation for the AddGroupCommandHandler.
/// </summary>
public class AddGroupCommandHandler : ICommandHandler<AddGroupCommand, GroupResult>
{
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    public AddGroupCommandHandler(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<GroupResult>> Handle(AddGroupCommand request, CancellationToken cancellationToken)
    {
        var groupResult = Domain.Group.Group.Create(
                   null,
                   request.Name,
                   new UserId(request.OwnerId));

        if (groupResult.IsSuccess)
        {
            var addResult = await groupRepository.AddAsync(groupResult.Value);
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
