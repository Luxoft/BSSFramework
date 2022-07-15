using System;

using Framework.Core;

using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public class AuthorizationBLLContextSettings : IAuthorizationBLLContextSettings
{
    public AuthorizationBLLContextSettings(ITypeResolver<string> securityTypeResolver)
    {
        this.SecurityTypeResolver = securityTypeResolver;
    }

    public ITypeResolver<string> SecurityTypeResolver { get; }


    public ITypeResolver<string> TypeResolver { get; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();
}
