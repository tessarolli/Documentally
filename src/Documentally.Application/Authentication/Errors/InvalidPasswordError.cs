using FluentResults;
using FluentResults.StatusCodes.Errors;

namespace Documentally.Application.Authentication.Errors;

public class InvalidPasswordError : UnauthorizedError
{
    public InvalidPasswordError(string message = "Invalid Password") : base(message)
    {
    }

}
