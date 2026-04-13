using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.BLL;

public interface ITargetSystemTypeResolverContainer
{
    ITypeResolver<TypeNameIdentity> TypeResolver { get; }
}
