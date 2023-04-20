using Documentally.Application.Authentication.Common;
using Documentally.Application.Authentication.Errors;
using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Application.Interfaces.Persistence;
using Documentally.Domain.Entities;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // Check if User with given e-mail already exists
            if (_userRepository.GetByEmail(query.Email) is not User user)
            {
                return Result.Fail(new UserWithEmailNotFoundError());
            }

            // Validate the Password
            if (user.Password != query.Password)
            {
                return Result.Fail(new InvalidPasswordError());
            }

            // Generate Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
