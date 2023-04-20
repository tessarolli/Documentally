// <copyright file="RegisterCommandHandler.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>
namespace Documentally.Application.Authentication.Commands.Register
{
    using Documentally.Application.Authentication.Common;
    using Documentally.Application.Authentication.Errors;
    using Documentally.Application.Interfaces.Infrastructure;
    using Documentally.Application.Interfaces.Persistence;
    using Documentally.Domain.Entities;
    using FluentResults;
    using MediatR;

    /// <summary>
    /// The implementation for the Resgister Command.
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">UserRepository being injected.</param>
        /// <param name="jwtTokenGenerator">JwtTokenGenerator being injected.</param>
        public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The actual command Handler.
        /// </summary>
        /// <param name="command">RegisterCommand.</param>
        /// <param name="cancellationToken">Async CancellationToken.</param>
        /// <returns>FluentResult for the operation.</returns>
        public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // Check if User with given e-mail already exists
            if (this.userRepository.GetByEmail(command.Email) is not null)
            {
                return Result.Fail(new UserWithEmailAlreadyExistsError());
            }

            // Create a new user
            var user = User.Create(command.FirstName, command.LastName, command.Email, command.Password);

            // Add to the Database
            this.userRepository.Add(user);

            // Generate Token
            var token = this.jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
