// <copyright file="IQueryHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using MediatR;

namespace Documentally.Application.Abstractions.Messaging;

/// <summary>
/// IQueryHandler Interface.
/// </summary>
/// <typeparam name="TQuery">Type of Query.</typeparam>
/// <typeparam name="TResponse">Type of response.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}