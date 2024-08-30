using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class ConditionFilterBuilder<TDomainObject>(
    SecurityPath<TDomainObject>.ConditionPath securityPath)
    : AccessorsFilterBuilder<TDomainObject>
{
    public override Expression<Func<IPermission, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
    {
        var hasAccess = securityPath.SecurityFilter.Eval(domainObject, LambdaCompileCache);

        return _ => hasAccess;
    }

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
