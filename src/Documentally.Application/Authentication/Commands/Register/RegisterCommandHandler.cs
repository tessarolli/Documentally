using Documentally.Application.Authentication.Common;
using Documentally.Application.Authentication.Errors;
using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Application.Interfaces.Persistence;
using Documentally.Domain.Entities;
using FluentResults;
using MediatR;

namespace Documentally.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // Check if User with given e-mail already exists
            if (_userRepository.GetByEmail(command.Email) is not null)
            {
                return Result.Fail(new UserWithEmailAlreadyExistsError());
            }

            // Create a new user
            var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

            // Add to the Database
            _userRepository.Add(user);

            // Generate Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
