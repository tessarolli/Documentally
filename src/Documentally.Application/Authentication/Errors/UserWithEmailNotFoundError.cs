using FluentResults;
using FluentResults.StatusCodes.Errors;

namespace Documentally.Application.Authentication.Errors;

public class UserWithEmailNotFoundError : NotFoundError
{
    public UserWithEmailNotFoundError(string message = "Account with given e-mail address does not exist") : base(message)
    {
    }
}
