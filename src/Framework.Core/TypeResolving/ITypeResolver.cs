using Framework.Core.TypeResolving.TypeSource;

namespace Framework.Core.TypeResolving;

public interface ITypeResolver<in T> : ITypeSource
{
    Type Resolve(T identity);
}
