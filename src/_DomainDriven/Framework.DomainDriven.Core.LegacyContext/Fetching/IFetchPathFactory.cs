using Framework.Core;

namespace Framework.DomainDriven;

public interface IFetchPathFactory<in T>
{
    IEnumerable<PropertyPath> Create(Type startDomainType, T rule);
}
