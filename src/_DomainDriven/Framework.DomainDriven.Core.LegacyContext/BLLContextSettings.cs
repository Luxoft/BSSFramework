using Framework.Core;

namespace Framework.DomainDriven;

public class BLLContextSettings<TPersistentDomainObjectBase> : ITypeResolverContainer<string>
{
    public ITypeResolver<string> TypeResolver { get; init; } = TypeSource.FromSample<TPersistentDomainObjectBase>().ToDefaultTypeResolver();
}
