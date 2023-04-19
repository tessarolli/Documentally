using Documentally.Application.Interfaces.Infrastructure;
using Documentally.Application.Interfaces.Persistence;
using Documentally.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentally.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            // Check if User with given e-mail already exists
            if (_userRepository.GetByEmail(email) is not null)
            {
                throw new InvalidOperationException("User already exists");
            }

            // Create a new user
            var user = User.Create(firstName, lastName, email, password);

            // Add to the Database
            _userRepository.Add(user);

            // Generate Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }

        AuthenticationResult IAuthenticationService.Login(string email, string password)
        {
            // Check if User with given e-mail already exists
            if (_userRepository.GetByEmail(email) is not User user)
            {
                throw new InvalidOperationException("User with given e-mail does not exist");
            }

            // Validate the Password
            if (user.Password != password)
            {
                throw new InvalidOperationException("Invalid Password");
            }

            // Generate Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }


    }
}
