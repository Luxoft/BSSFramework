using JetBrains.Annotations;

namespace Framework.DomainDriven;

public abstract class MainFetchServiceBase<TPersistentDomainObjectBase> : IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule>
{
    protected MainFetchServiceBase()
    {
    }

    public IFetchContainer<TDomainObject> GetContainer<TDomainObject>([NotNull] FetchBuildRule.DTOFetchBuildRule rule)
            where TDomainObject : TPersistentDomainObjectBase
    {
        if (rule == null) throw new ArgumentNullException(nameof(rule));

        return this.GetContainer<TDomainObject>(rule.DTOType);
    }

    protected abstract Framework.DomainDriven.IFetchContainer<TDomainObject> GetContainer<TDomainObject>(Framework.Transfering.ViewDTOType rule);
}
