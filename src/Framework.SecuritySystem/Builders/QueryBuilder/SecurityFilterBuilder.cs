using System.Linq.Expressions;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public abstract class SecurityFilterBuilder<TDomainObject>
{
    public abstract Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType);
}
