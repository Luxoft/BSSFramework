using Framework.Core.TypeResolving;
using Framework.Core.TypeResolving.TypeSource;

namespace Framework.BLL;

public class BLLContextSettings<TPersistentDomainObjectBase> : ITypeResolverContainer<string>
{
    public ITypeResolver<string> TypeResolver { get; init; } = TypeSource.FromSample<TPersistentDomainObjectBase>().ToDefaultTypeResolver();
}
