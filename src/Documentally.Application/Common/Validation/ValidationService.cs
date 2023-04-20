using FluentResults;
using FluentValidation;
using MediatR;

namespace Documentally.Application.Common.Validation;

public class ValidationService
{
    private readonly IEnumerable<IValidator> _fluentValidators;

    public ValidationService(IEnumerable<IValidator> fluentValidators)
    {
        _fluentValidators = fluentValidators;
    }

    public Result ValidateCommand<TCommand>(TCommand command) where TCommand : IRequest<Result>
    {
        var result = Result.Ok();

        if (!_fluentValidators.Any())
        {
            return result;
        }

        var context = new ValidationContext<TCommand>(command);

        var failures = _fluentValidators
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