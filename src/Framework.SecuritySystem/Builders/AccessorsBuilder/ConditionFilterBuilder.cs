using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class ConditionFilterBuilder<TPermission, TDomainObject>(
    SecurityPath<TDomainObject>.ConditionPath securityPath)
    : AccessorsFilterBuilder<TPermission, TDomainObject>
{
    public override Expression<Func<TPermission, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
    {
        var hasAccess = securityPath.SecurityFilter.Eval(domainObject, LambdaCompileCache);

        return _ => hasAccess;
    }

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
