// <copyright file="User.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Domain.Entities
{
    using Documentally.Domain.BaseClasses.DDD;
    using Documentally.Domain.ValueObjects;

    /// <summary>
    /// User Entity.
    /// </summary>
    public class User : Entity<UserId>
    {
        /// <summary>
        /// Gets User's First Name.
        /// </summary>
        public string FirstName { get; private set; } = null!;

        /// <summary>
        /// Gets User's Last Name.
        /// </summary>
        public string LastName { get; private set; } = null!;

        /// <summary>
        /// Gets User's Last Name.
        /// </summary>
        public string Email { get; private set; } = null!;

        /// <summary>
        /// Gets User's Last Name.
        /// </summary>
        public string Password { get; private set; } = null!;

        /// <summary>
        /// Gets User's Last Name.
        /// </summary>
        public string Role { get; private set; } = null!;

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="firstName">User First Name.</param>
        /// <param name="lastName">User Last Name.</param>
        /// <param name="email">User Email.</param>
        /// <param name="password">User Password.</param>
        /// <returns>new User instance.</returns>
        public static User Create(string firstName, string lastName, string email, string password)
        {
            return new User
            {
                Id = new UserId(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
            };
        }
    }
}