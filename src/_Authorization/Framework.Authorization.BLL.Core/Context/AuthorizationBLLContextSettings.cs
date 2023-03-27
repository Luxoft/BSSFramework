using Framework.Core;

using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public class AuthorizationBLLContextSettings : IAuthorizationBLLContextSettings
{
    public ITypeResolver<string> TypeResolver { get; init; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();
}
