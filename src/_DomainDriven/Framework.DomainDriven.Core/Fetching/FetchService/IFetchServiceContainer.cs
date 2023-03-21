namespace Framework.DomainDriven;

public interface IFetchServiceContainer<in TPersistentDomainObjectBase, in TBuildRule>
{
    IFetchService<TPersistentDomainObjectBase, TBuildRule> FetchService { get; }
}
