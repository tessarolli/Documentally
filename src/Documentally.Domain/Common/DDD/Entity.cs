// <copyright file="Entity.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;
using FluentResults;
using FluentValidation;

namespace Documentally.Domain.Common.DDD;

/// <summary>
/// An abstract class that should be implemented to represent an Entity of the Domain.
/// </summary>
/// <typeparam name="TId">The Type of the Id value object.</typeparam>
public abstract class Entity<TId> : IEntity, IEquatable<Entity<TId>>
    where TId : notnull
{
    private readonly List<IDomainEvent> domainEvents = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">Entity Id.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets the Identificator of this entity.
    /// </summary>
    public TId Id { get; protected set; }

    /// <inheritdoc/>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.ToList();

    /// <summary>
    /// Equality operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have the same Ids.</returns>
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Not Equal operator.
    /// </summary>
    /// <param name="left">Left entity.</param>
    /// <param name="right">Right entity.</param>
    /// <returns>True if both entities have different Ids.</returns>
    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    /// <inheritdoc/>
    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    /// <inheritdoc/>
    public object GetId()
    {
        return Id;
    }

    /// <inheritdoc/>
    public void RaiseDomainEvent(IDomainEvent @event)
    {
        domainEvents.Add(@event);
    }

    /// <inheritdoc/>
    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    /// <summary>
    /// Gets the IValidator for this entity.
    /// </summary>
    /// <returns>IValidator for this entity.</returns>
    protected abstract object GetValidator();

    /// <summary>
    /// Perform Validation on the entity.
    /// </summary>
    /// <returns>Result with Success or Failure status.</returns>
    protected Result Validate()
    {
        var validatorObject = GetValidator();
        if (validatorObject is not AbstractValidator<Entity<TId>> validator)
        {
            return Result.Ok();
        }

        var validationResult = validator.Validate(this);

        if (validationResult.IsValid)
        {
            return Result.Ok();
        }
        else
        {
            return Result.Fail(
                validationResult.Errors
                    .Where(validationFailure => validationFailure is not null)
                    .Select(failure => new ValidationError(
                       failure.ErrorMessage,
                       new Error(failure.PropertyName)))
                    .Distinct());
        }
    }
}