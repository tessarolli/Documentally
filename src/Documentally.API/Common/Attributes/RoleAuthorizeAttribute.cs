// <copyright file="RoleAuthorizeAttribute.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Documentally.API.Common.Attributes;

/// <summary>
/// Role Based Authorization support.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="role">The required role.</param>
    public RoleAuthorizeAttribute(Roles role)
    {
        Roles = role.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="roles">The array of required roles.</param>
    public RoleAuthorizeAttribute(Roles[] roles)
    {
        Roles = string.Join(',', roles);
    }
}