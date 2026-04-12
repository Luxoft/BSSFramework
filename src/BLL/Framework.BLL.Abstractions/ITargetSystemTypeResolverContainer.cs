using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.BLL;

public interface ITargetSystemTypeResolverContainer
{
    ITypeResolver<string> TypeResolverS { get; }

    ITypeResolver<TypeNameIdentity> TypeResolver { get; }
}
