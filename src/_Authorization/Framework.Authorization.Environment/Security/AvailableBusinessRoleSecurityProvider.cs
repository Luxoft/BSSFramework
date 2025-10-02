using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystemImpl;

using SecuritySystem;
using SecuritySystem.Providers;
using SecuritySystem.RelativeDomainPathInfo;
using SecuritySystem.SecurityAccessor;

namespace Framework.Authorization.Environment.Security;

public class AvailableBusinessRoleSecurityProvider<TDomainObject>(
    IExpressionEvaluator expressionEvaluator,
    IAvailablePermissionSource availablePermissionSource,
    IRelativeDomainPathInfo<TDomainObject, BusinessRole> toBusinessRolePathInfo)
    : SecurityProvider<TDomainObject>(expressionEvaluator)
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =

        availablePermissionSource
            .GetAvailablePermissionsQueryable(DomainSecurityRule.AnyRole)
            .Pipe(
                permissionQ =>
                    ExpressionHelper.Create((BusinessRole businessRole) => permissionQ.Select(p => p.Role).Contains(businessRole)))
            .Pipe(toBusinessRolePathInfo.CreateCondition);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        return toBusinessRolePathInfo.GetRelativeObjects(domainObject).Select(this.GetAccessorData).Or();
    }

    private SecurityAccessorData GetAccessorData(BusinessRole businessRole)
    {
        return SecurityAccessorData.Return(
            availablePermissionSource
                .GetAvailablePermissionsQueryable(DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() })
                .Where(permission => permission.Role == businessRole)
                .Select(permission => permission.Principal)
                .Distinct()
                .Select(principal => principal.Name));
    }
}
