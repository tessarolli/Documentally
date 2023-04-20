using Documentally.Application.Common.Validation.Errors;

namespace Documentally.Application.Authentication.Errors;

public class UserWithEmailNotFoundError : UnauthorizedError
{
    public UserWithEmailNotFoundError() 
    {
        Message = "Account with given e-mail address does not exist";
    }
}
