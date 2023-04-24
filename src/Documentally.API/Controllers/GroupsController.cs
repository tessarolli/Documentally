// <copyright file="GroupsController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Groups.Commands.AddGroup;
using Documentally.Application.Groups.Commands.AddUserToGroup;
using Documentally.Application.Groups.Commands.DeleteGroup;
using Documentally.Application.Groups.Commands.RemoveUserFromGroup;
using Documentally.Application.Groups.Commands.UpdateGroup;
using Documentally.Application.Groups.Queries.GetGroupById;
using Documentally.Application.Groups.Queries.GetGroupsList;
using Documentally.Contracts.Group.Requests;
using Documentally.Contracts.Group.Responses;
using Documentally.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Groups Controller.
/// </summary>
[Route("[controller]")]
public class GroupsController : ResultControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    public GroupsController(IMediator mediator, IMapper mapper, ILogger<GroupsController> logger)
        : base(logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets a list of Groups.
    /// </summary>
    /// <returns>The list of Groups.</returns>
    [HttpGet]
    [RoleAuthorize]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroups()
    {
        logger.LogInformation("GET /Groups/ called");

        var groupsListResult = await mediator.Send(new GetGroupsListQuery());

        return ValidateResult(
            groupsListResult,
            () => Ok(mapper.Map<List<GroupResponse>>(groupsListResult.Value)),
            () => Problem());
    }

    /// <summary>
    /// Gets a Group by its Id.
    /// </summary>
    /// <param name="id">Group Id.</param>
    /// <returns>The Group Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<ActionResult<GroupResponse>> GetGroupById(long id)
    {
        logger.LogInformation("GET /Groups/Id called");

        var result = await mediator.Send(new GetGroupByIdQuery(id));

        return ValidateResult(
            result,
            () => Ok(mapper.Map<GroupResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Add a group to the Group Repository.
    /// </summary>
    /// <param name="request">Group data.</param>
    /// <returns>The group instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<GroupResponse>> AddGroup(AddGroupRequest request)
    {
        logger.LogInformation("POST /Groups called");

        var addGroupCommand = mapper.Map<AddGroupCommand>(request);

        var groupResult = await mediator.Send(addGroupCommand);

        return ValidateResult(
            groupResult,
            () => Ok(mapper.Map<GroupResponse>(groupResult.Value)),
            () => Problem());
    }

    /// <summary>
    /// Updates a group in the Group Repository.
    /// </summary>
    /// <param name="request">Group data.</param>
    /// <returns>The group instance created with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<GroupResponse>> UpdateGroup(UpdateGroupRequest request)
    {
        logger.LogInformation("PUT /Groups called");

        var updateGroupCommand = mapper.Map<UpdateGroupCommand>(request);

        var groupResult = await mediator.Send(updateGroupCommand);

        return ValidateResult(
            groupResult,
            () => Ok(mapper.Map<GroupResponse>(groupResult.Value)),
            () => Problem());
    }

    /// <summary>
    /// Deletes a group from the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> DeleteGroup(DeleteGroupRequest request)
    {
        logger.LogInformation("DELETE /Groups called");

        var deleteGroupCommand = mapper.Map<DeleteGroupCommand>(request);

        var groupResult = await mediator.Send(deleteGroupCommand);

        return ValidateResult<object>(
            groupResult,
            () => Ok(),
            () => Problem());
    }

    /// <summary>
    /// Add an user to the Group in the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpPost("user")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<GroupResponse>> AddUserToGroup(AddUserToGroupRequest request)
    {
        logger.LogInformation("PUT /Groups/user called");

        var command = mapper.Map<AddUserToGroupCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<GroupResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Removes an user from the Group in the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete("user")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<GroupResponse>> RemoveUserFromGroup(RemoveUserFromGroupRequest request)
    {
        logger.LogInformation("DELETE /Groups/user called");

        var command = mapper.Map<RemoveUserFromGroupCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<GroupResponse>(result.Value)),
            () => Problem());
    }
}
