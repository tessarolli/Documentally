//using Documentally.Application.Authentication.Commands.Register;
//using Documentally.Application.Authentication.Common;
//using FluentResults;
//using FluentValidation;
//using FluentValidation.Results;
//using MediatR;

//namespace Documentally.Application.Authentication.Behaviors;

//public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, Result<AuthenticationResult>>
//{
//    private readonly IValidator<RegisterCommand> _validator;

//    public ValidateRegisterCommandBehavior(IValidator<RegisterCommand> validator)
//    {
//        _validator = validator;
//    }

//    public async Task<Result<AuthenticationResult>> Handle(RegisterCommand request, RequestHandlerDelegate<Result<AuthenticationResult>> next, CancellationToken cancellationToken)
//    {
//        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

//        if (validationResult.IsValid)
//        {
//            return await next();
//        }

//        var errors = validationResult.Errors
//                .ConvertAll(e => new ValidationFailure(e.PropertyName, e.ErrorMessage));

//        // arrumar isso aqui.. 
//        var errorResult = new Result<AuthenticationResult>();

//        return errorResult;
//    }
//}
