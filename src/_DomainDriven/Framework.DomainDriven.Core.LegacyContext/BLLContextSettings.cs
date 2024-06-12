using Framework.Core;

namespace Framework.DomainDriven;

public class BLLContextSettings<PersistentDomainObjectBase> : ITypeResolverContainer<string>
{
    public ITypeResolver<string> TypeResolver { get; init; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();
}
