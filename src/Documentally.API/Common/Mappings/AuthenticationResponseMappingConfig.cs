using Documentally.Application.Authentication.Common;
using Documentally.Contracts.Authentication;
using Mapster;

namespace Documentally.API.Common.Mappings;

public class AuthenticationResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest, src => src.User);
    }
}
