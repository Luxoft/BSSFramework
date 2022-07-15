using Framework.Core;

namespace Framework.Authorization.BLL;

public interface IAuthorizationBLLContextSettings : ITypeResolverContainer<string>
{
    ITypeResolver<string> SecurityTypeResolver { get; }
}
