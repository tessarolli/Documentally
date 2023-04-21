// <copyright file="RequestValidationBehavior.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Messaging;
using Documentally.Domain.BaseClasses;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Documentally.Application.Common.Validation;

/// <summary>
/// Provides a generic Validation Behavior for incoming requests.
/// </summary>
/// <typeparam name="TRequest">The type of the incoming request contract.</typeparam>
/// <typeparam name="TResponse">The type of the request's response contract.</typeparam>
public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Injected validators.</param>
    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        ValidationError[] errors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new ValidationError(
                failure.ErrorMessage,
                new Error(failure.PropertyName)))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return (TResponse)typeof(Result<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResponse).GenericTypeArguments[0])
                .GetMethods()
                .First(x => x.Name == "WithErrors")
                .Invoke(Activator.CreateInstance(typeof(TResponse)), new object?[] { errors }) !;
        }

        return await next();
    }
}
