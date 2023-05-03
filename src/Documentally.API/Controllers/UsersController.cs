// <copyright file="UsersController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Users.Commands.AddUser;
using Documentally.Application.Users.Commands.DeleteUser;
using Documentally.Application.Users.Commands.UpdateUser;
using Documentally.Application.Users.Queries.GetUserById;
using Documentally.Application.Users.Queries.GetUsersList;
using Documentally.Application.Users.Results;
using Documentally.Contracts.User.Requests;
using Documentally.Contracts.User.Responses;
using Documentally.Domain.Enums;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Users Controller.
/// </summary>
[Route("[controller]")]
public class UsersController : ResultControllerBase<UsersController>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    /// <param name="exceptionHandlingService">Injected exceptionHandlingService.</param>
    public UsersController(IMediator mediator, IMapper mapper, ILogger<UsersController> logger, IExceptionHandlingService exceptionHandlingService)
        : base(mediator, mapper, logger, exceptionHandlingService)
    {
    }

    /// <summary>
    /// Gets a list of Users.
    /// </summary>
    /// <returns>The list of Users.</returns>
    [HttpGet]
    [RoleAuthorize]
    public async Task<IActionResult> GetUsers() =>
        await HandleRequestAsync<GetUsersListQuery, List<UserResult>, List<UserResponse>>();

    /// <summary>
    /// Gets a User by its Id.
    /// </summary>
    /// <param name="id">User Id.</param>
    /// <returns>The User Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<IActionResult> GetUserById(long id) =>
        await HandleRequestAsync<GetUserByIdQuery, UserResult, UserResponse>(id);

    /// <summary>
    /// Add a User to the User Repository.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>The User instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> AddUser(AddUserRequest request) =>
        await HandleRequestAsync<AddUserCommand, UserResult, UserResponse>(request);

    /// <summary>
    /// Updates a User in the User Repository.
    /// </summary>
    /// <param name="request">User data.</param>
    /// <returns>The User instance updated with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest request) =>
        await HandleRequestAsync<UpdateUserCommand, UserResult, UserResponse>(request);

    /// <summary>
    /// Deletes a User from the User Repository.
    /// </summary>
    /// <param name="request">User Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> DeleteUser(DeleteUserRequest request) =>
        await HandleRequestAsync<DeleteUserCommand, Result, object>(request);
}
