using Documentally.Application.Common.Validation.Errors;

namespace Documentally.Application.Authentication.Errors;

public class InvalidPasswordError : UnauthorizedError
{
    public InvalidPasswordError()
    {
        Message = "Invalid Password";
    }

}
