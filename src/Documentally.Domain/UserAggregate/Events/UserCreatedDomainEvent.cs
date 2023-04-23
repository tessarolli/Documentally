// <copyright file="UserCreatedDomainEvent.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;

namespace Documentally.Domain.UserAggregate.Events;

/// <summary>
/// Event that occurs after the user registration proccess is concluded with success.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "Reviewed")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "Reviewed")]
public record UserCreatedDomainEvent(User User) : IDomainEvent;