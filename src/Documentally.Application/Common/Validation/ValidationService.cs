// <copyright file="ValidationService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using FluentResults;
using FluentValidation;
using MediatR;

namespace Documentally.Application.Common.Validation;

/// <summary>
/// Validation Service Class for Performing Fluent Validations of Commands.
/// </summary>
public class ValidationService
{
    private readonly IEnumerable<IValidator> fluentValidators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationService"/> class.
    /// </summary>
    /// <param name="fluentValidators">List of Validators for beeing validated.</param>
    public ValidationService(IEnumerable<IValidator> fluentValidators)
    {
        this.fluentValidators = fluentValidators;
    }

    /// <summary>
    /// Method to perform the validations of each IValidator.
    /// </summary>
    /// <typeparam name="TCommand">The type of the Command being validated.</typeparam>
    /// <param name="command">Command name.</param>
    /// <returns>FluentResult with the results of the validations.</returns>
    public Result ValidateCommand<TCommand>(TCommand command)
        where TCommand : IRequest<Result>
    {
        var result = Result.Ok();

        if (!fluentValidators.Any())
        {
            return result;
        }

        var context = new ValidationContext<TCommand>(command);

        var failures = fluentValidators
            .Select(v => v.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
        {
            return result;
        }

        return Result
            .Fail("Validation failures")
            .WithErrors(failures
                .Select(x => new Error(x.ErrorMessage)));
    }
}