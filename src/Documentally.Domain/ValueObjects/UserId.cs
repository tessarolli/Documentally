// <copyright file="UserId.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.BaseClasses.DDD;

namespace Documentally.Domain.ValueObjects
{
    /// <summary>
    /// User Id Value Object.
    /// </summary>
    public sealed class UserId : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserId"/> class.
        /// </summary>
        /// <param name="guid">Guid value if exists.</param>
        public UserId(Guid? guid = null)
        {
            if (guid is null)
            {
                Value = Guid.NewGuid();
            }
            else
            {
                Value = guid.Value;
            }
        }

        /// <summary>
        /// Gets the User ID.
        /// </summary>
        public Guid Value { get; }

        /// <summary>
        /// Method required for comparing value objects.
        /// </summary>
        /// <returns>An ienumerable with all the properties of the value object.</returns>
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
