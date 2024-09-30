using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public class ConditionSecurityProvider<TDomainObject>(
    Expression<Func<TDomainObject, bool>> securityFilter,
    LambdaCompileMode securityFilterCompileMode)
    : SecurityProvider<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter => securityFilter;

    protected override LambdaCompileMode SecurityFilterCompileMode => securityFilterCompileMode;

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        this.HasAccess(domainObject) ? SecurityAccessorData.Infinity : SecurityAccessorData.Empty;
}
