// <copyright file="DeleteGroupCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Application.Abstractions.Repositories;
using Documentally.Domain.Group.ValueObjects;
using FluentResults;

namespace Documentally.Application.Groups.Commands.DeleteGroup;

/// <summary>
/// Implementation for the Login Query.
/// </summary>
public class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand>
{
    private readonly IGroupRepository groupRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteGroupCommandHandler"/> class.
    /// </summary>
    /// <param name="groupRepository">Injected UserRepository.</param>
    public DeleteGroupCommandHandler(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    /// <inheritdoc/>
    public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        return await groupRepository.RemoveAsync(new GroupId(request.Id));
    }
}
