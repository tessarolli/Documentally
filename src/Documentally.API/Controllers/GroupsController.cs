// <copyright file="GroupsController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Groups.Commands.AddGroup;
using Documentally.Application.Groups.Commands.AddUserToGroup;
using Documentally.Application.Groups.Commands.DeleteGroup;
using Documentally.Application.Groups.Commands.RemoveUserFromGroup;
using Documentally.Application.Groups.Commands.UpdateGroup;
using Documentally.Application.Groups.Queries.GetGroupById;
using Documentally.Application.Groups.Queries.GetGroupsList;
using Documentally.Application.Groups.Results;
using Documentally.Contracts.Group.Requests;
using Documentally.Contracts.Group.Responses;
using Documentally.Domain.Enums;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Groups Controller.
/// </summary>
[Route("[controller]")]
public class GroupsController : ResultControllerBase<GroupsController>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupsController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    /// <param name="exceptionHandlingService">Injected exceptionHandlingService.</param>
    public GroupsController(IMediator mediator, IMapper mapper, ILogger<GroupsController> logger, IExceptionHandlingService exceptionHandlingService)
        : base(mediator, mapper, logger, exceptionHandlingService)
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
    public async Task<IActionResult> GetGroups() =>
        await HandleRequestAsync<GetGroupsListQuery, List<GroupResult>, List<GroupResponse>>();

    /// <summary>
    /// Gets a Group by its Id.
    /// </summary>
    /// <param name="id">Group Id.</param>
    /// <returns>The Group Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<IActionResult> GetGroupById(long id) =>
        await HandleRequestAsync<GetGroupByIdQuery, GroupResult, GroupResponse>(id);

    /// <summary>
    /// Add a group to the Group Repository.
    /// </summary>
    /// <param name="request">Group data.</param>
    /// <returns>The group instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> AddGroup(AddGroupRequest request) =>
        await HandleRequestAsync<AddGroupCommand, GroupResult, GroupResponse>(request);

    /// <summary>
    /// Updates a group in the Group Repository.
    /// </summary>
    /// <param name="request">Group data.</param>
    /// <returns>The group instance created with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> UpdateGroup(UpdateGroupRequest request) =>
        await HandleRequestAsync<UpdateGroupCommand, GroupResult, GroupResponse>(request);

    /// <summary>
    /// Deletes a group from the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> DeleteGroup(DeleteGroupRequest request) =>
        await HandleRequestAsync<DeleteGroupCommand, Result, object>(request);

    /// <summary>
    /// Add an user to the Group in the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpPost("user")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> AddUserToGroup(AddUserToGroupRequest request) =>
        await HandleRequestAsync<AddUserToGroupCommand, GroupResult, GroupResponse>(request);

    /// <summary>
    /// Removes an user from the Group in the Group Repository.
    /// </summary>
    /// <param name="request">Group Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete("user")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> RemoveUserFromGroup(RemoveUserFromGroupRequest request) =>
        await HandleRequestAsync<RemoveUserFromGroupCommand, GroupResult, GroupResponse>(request);
}
