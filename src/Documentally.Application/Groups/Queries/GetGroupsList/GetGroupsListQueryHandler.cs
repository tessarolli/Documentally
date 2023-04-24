// <copyright file="GetGroupsListQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Application.Groups.Results;
using FluentResults;

namespace Documentally.Application.Groups.Queries.GetGroupsList;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class GetGroupsListQueryHandler : IQueryHandler<GetGroupsListQuery, List<GroupResult>>
{
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupsListQueryHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    public GetGroupsListQueryHandler(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    /// <inheritdoc/>
    public async Task<Result<List<GroupResult>>> Handle(GetGroupsListQuery query, CancellationToken cancellationToken)
    {
        var result = new List<GroupResult>();

        var groupsResult = await groupRepository.GetAllAsync();
        if (groupsResult.IsSuccess)
        {
            foreach (var group in groupsResult.Value)
            {
                result.Add(new GroupResult(
                    group.Id.Value,
                    group.Name,
                    group.OwnerId.Value,
                    group.MemberIds.Select(x => x.Value).ToList(),
                    group.CreatedAtUtc));
            }
        }

        return Result.Ok(result);
    }
}