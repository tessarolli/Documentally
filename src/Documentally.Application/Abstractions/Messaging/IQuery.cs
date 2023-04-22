// <copyright file="IQuery.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using MediatR;

namespace Documentally.Application.Abstractions.Messaging;

/// <summary>
/// IQuery Interface.
/// </summary>
/// <typeparam name="TResponse">Type of response.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}