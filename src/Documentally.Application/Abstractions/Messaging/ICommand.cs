// <copyright file="ICommand.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using MediatR;

namespace Documentally.Application.Abstractions.Messaging;

/// <summary>
/// ICommand Interface.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// ICommand Interface with Result of TResponse.
/// </summary>
/// <typeparam name="TResponse">Type of response.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}