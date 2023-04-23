// <copyright file="GetGroupByIdQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Group.Results;
using Documentally.Domain.GroupAggregate.ValueObjects;
using FluentResults;

namespace Documentally.Application.Group.Queries.GetGroupById;

/// <summary>
/// Implementation for the GetGroupByIdQuery.
/// </summary>
public class GetGroupByIdQueryHandler : IQueryHandler<GetGroupByIdQuery, GroupResult>
{
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    public GetGroupByIdQueryHandler(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<GroupResult>> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
    {
        var groupResult = await groupRepository.GetByIdAsync(new GroupId(query.Id));
        if (groupResult.IsSuccess)
        {
           return Result.Ok(new GroupResult(
                groupResult.Value.Id.Value,
                groupResult.Value.Name,
                groupResult.Value.OwnerId.Value,
                groupResult.Value.MemberIds.Select(x => x.Value).ToList(),
                groupResult.Value.CreatedAtUtc));
        }

        return Result.Fail(groupResult.Errors);
    }
}