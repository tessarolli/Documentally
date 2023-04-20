using FluentResults;
using FluentValidation;
using MediatR;

namespace Documentally.Application.Common.Validation;

public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<Result>
       where TResponse : Result
{
    private readonly ValidationService _validationService;

    public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validationService = new ValidationService(validators);
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = _validationService.ValidateCommand(command);

        if (validationResult.IsFailed)
        {
            return (TResponse)validationResult;
        }

        var handlerResult = await next();

        var result = Result.Merge(validationResult, handlerResult);

        return (TResponse)result;
    }
}
