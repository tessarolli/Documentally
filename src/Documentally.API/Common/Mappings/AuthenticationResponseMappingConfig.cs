// <copyright file="AuthenticationResponseMappingConfig.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Authentication.Results;
using Documentally.Contracts.Authentication;
using Mapster;

namespace Documentally.API.Common.Mappings;

/// <summary>
/// Authentication Mapster Config Mapping.
/// </summary>
public class AuthenticationResponseMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.Id, src => src.User.Id.Value)
            .Map(dest => dest, src => src.User);
    }
}
