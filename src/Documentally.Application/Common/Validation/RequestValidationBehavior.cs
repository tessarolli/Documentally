// <copyright file="RequestValidationBehavior.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using FluentValidation;
using MediatR;

namespace Documentally.Application.Common.Validation;

/// <summary>
/// Provides a generic Validation Behavior for incoming requests.
/// </summary>
/// <typeparam name="TRequest">The type of the incoming request contract.</typeparam>
/// <typeparam name="TResponse">The type of the request's response contract.</typeparam>
public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<Result>
       where TResponse : Result
{
    private readonly ValidationService validationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Injected validators.</param>
    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        validationService = new ValidationService(validators);
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = validationService.ValidateCommand(command);

        if (validationResult.IsFailed)
        {
            return (TResponse)validationResult;
        }

        var handlerResult = await next();

        var result = Result.Merge(validationResult, handlerResult);

        return (TResponse)result;
    }
}
