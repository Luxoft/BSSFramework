using CommonFramework;
using CommonFramework.IdentitySource;

namespace Framework.DomainDriven.NHibernate;

public class DomainObjectSaveStrategyServiceProxyBinder<TDomainObject>(IIdentityInfo<TDomainObject> identityInfo) : IServiceProxyBinder
{
    public Type GetTargetServiceType() =>
        typeof(DomainObjectSaveStrategy<,>).MakeGenericType(identityInfo.DomainObjectType, identityInfo.IdentityType);
}
