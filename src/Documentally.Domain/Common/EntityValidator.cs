// <copyright file="EntityValidator.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;
using FluentValidation;

namespace Documentally.Domain.Common;

/// <summary>
/// Abstract Base Class for Domain Entities Validators.
/// </summary>
/// <typeparam name="T">Type of the Domain Entity.</typeparam>
/// <typeparam name="TId">Type of the Id Property.</typeparam>
public abstract class EntityValidator<T, TId> : AbstractValidator<Entity<TId>>
where T : Entity<TId>
where TId : notnull
{
}
