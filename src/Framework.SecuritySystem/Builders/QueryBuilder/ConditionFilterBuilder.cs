using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class ConditionFilterBuilder<TPermission, TDomainObject>(
    SecurityPath<TDomainObject>.ConditionPath securityPath)
    : SecurityFilterBuilder<TPermission, TDomainObject>
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
        HierarchicalExpandType expandType)
    {
        var securityFilter = securityPath.SecurityFilter;

        return (domainObject, _) => securityFilter.Eval(domainObject);
    }
}
