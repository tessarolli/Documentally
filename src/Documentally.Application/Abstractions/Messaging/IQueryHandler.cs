// <copyright file="IQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using MediatR;

namespace Documentally.Application.Abstractions.Messaging;

/// <summary>
/// IQueryHandler Interface.
/// </summary>
/// <typeparam name="TQuery">Type of Query.</typeparam>
/// <typeparam name="TResponse">Type of response.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}