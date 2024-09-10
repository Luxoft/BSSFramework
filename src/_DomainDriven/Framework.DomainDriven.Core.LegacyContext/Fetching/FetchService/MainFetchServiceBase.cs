namespace Framework.DomainDriven;

public abstract class MainFetchServiceBase<TPersistentDomainObjectBase> : IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule>
{
    protected MainFetchServiceBase()
    {
    }

    public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(FetchBuildRule.DTOFetchBuildRule rule)
            where TDomainObject : TPersistentDomainObjectBase
    {
        if (rule == null) throw new ArgumentNullException(nameof(rule));

        return this.GetContainer<TDomainObject>(rule.DTOType);
    }

    protected abstract IFetchContainer<TDomainObject> GetContainer<TDomainObject>(Transfering.ViewDTOType rule);
}
