using Framework.Core;


namespace Framework.BLL.Fetching;

public interface IFetchPathFactory<in T>
{
    IEnumerable<PropertyPath> Create(Type startDomainType, T rule);
}
