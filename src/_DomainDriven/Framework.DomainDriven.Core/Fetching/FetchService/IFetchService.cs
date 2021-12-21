namespace Framework.DomainDriven
{
    public interface IFetchService<in TPersistentDomainObjectBase, in TBuildRule>
    {
        IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
            where TDomainObject : TPersistentDomainObjectBase;
    }
}