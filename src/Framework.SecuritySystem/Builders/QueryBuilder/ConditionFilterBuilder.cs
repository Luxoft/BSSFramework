using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class ConditionFilterBuilder<TDomainObject>(
    SecurityPath<TDomainObject>.ConditionPath securityPath)
    : SecurityFilterBuilder<TDomainObject>
{
    public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
        HierarchicalExpandType expandType)
    {
        var securityFilter = securityPath.SecurityFilter;

        return (domainObject, _) => securityFilter.Eval(domainObject);
    }
}
