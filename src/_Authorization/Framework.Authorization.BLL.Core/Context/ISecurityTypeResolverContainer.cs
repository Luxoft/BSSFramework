using Framework.Core;

namespace Framework.Authorization.BLL;

public interface ISecurityTypeResolverContainer
{
    ITypeResolver<string> SecurityTypeResolver
    {
        get;
    }
}
