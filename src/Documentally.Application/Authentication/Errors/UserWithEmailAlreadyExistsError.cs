using FluentResults;
using FluentResults.StatusCodes.Errors;

namespace Documentally.Application.Authentication.Errors;

public class UserWithEmailAlreadyExistsError : BusinessValidationError
{
    public UserWithEmailAlreadyExistsError(string message = "Given E-Mail address is already in use") : base(message)
    {
    }
}
