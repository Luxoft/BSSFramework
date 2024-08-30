using System.Linq.Expressions;

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public abstract class AccessorsFilterBuilder<TPermission, TDomainObject>
{
    public abstract Expression<Func<TPermission, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType);
}
