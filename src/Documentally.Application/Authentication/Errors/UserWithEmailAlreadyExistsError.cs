using Documentally.Application.Common.Validation.Errors;

namespace Documentally.Application.Authentication.Errors;

public class UserWithEmailAlreadyExistsError : UnauthorizedError
{
    public UserWithEmailAlreadyExistsError() 
    {
        Message = "Given E-Mail address is already in use";
    }
}
