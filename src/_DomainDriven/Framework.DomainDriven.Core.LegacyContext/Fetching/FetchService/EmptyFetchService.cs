namespace Framework.DomainDriven;

public class EmptyFetchService<TPersistentDomainObjectBase, TBuildRule> : IFetchService<TPersistentDomainObjectBase, TBuildRule>
{
    public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
        where TDomainObject : TPersistentDomainObjectBase
    {
        return FetchContainer<TDomainObject>.Empty;
    }
}
