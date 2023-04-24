// <copyright file="UsersController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Users.Commands.AddUser;
using Documentally.Application.Users.Commands.DeleteUser;
using Documentally.Application.Users.Commands.UpdateUser;
using Documentally.Application.Users.Queries.GetUserById;
using Documentally.Application.Users.Queries.GetUsersList;
using Documentally.Contracts.User.Requests;
using Documentally.Contracts.User.Responses;
using Documentally.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Users Controller.
/// </summary>
[Route("[controller]")]
public class UsersController : ResultControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    public UsersController(IMediator mediator, IMapper mapper, ILogger<UsersController> logger)
        : base(logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets a list of Users.
    /// </summary>
    /// <returns>The list of Users.</returns>
    [HttpGet]
    [RoleAuthorize]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        logger.LogInformation("GET /Users/ called");

        var result = await mediator.Send(new GetUsersListQuery());

        return ValidateResult(
            result,
            () => Ok(mapper.Map<List<UserResponse>>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Gets a User by its Id.
    /// </summary>
    /// <param name="id">User Id.</param>
    /// <returns>The User Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<ActionResult<UserResponse>> GetUserById(long id)
    {
        logger.LogInformation("GET /Users/Id called");

        var result = await mediator.Send(new GetUserByIdQuery(id));

        return ValidateResult(
            result,
            () => Ok(mapper.Map<UserResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Add a User to the User Repository.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>The User instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<UserResponse>> AddUser(AddUserRequest request)
    {
        logger.LogInformation("POST /Users called");

        var addUserCommand = mapper.Map<AddUserCommand>(request);

        var result = await mediator.Send(addUserCommand);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<UserResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Updates a User in the User Repository.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>The User instance created with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<UserResponse>> UpdateUser(UpdateUserRequest request)
    {
        logger.LogInformation("PUT /Users called");

        var updateUserCommand = mapper.Map<UpdateUserCommand>(request);

        var result = await mediator.Send(updateUserCommand);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<UserResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Deletes a User from the User Repository.
    /// </summary>
    /// <param name="request">User Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> DeleteUser(DeleteUserRequest request)
    {
        logger.LogInformation("DELETE /Users called");

        var deleteUserCommand = mapper.Map<DeleteUserCommand>(request);

        var result = await mediator.Send(deleteUserCommand);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }
}
