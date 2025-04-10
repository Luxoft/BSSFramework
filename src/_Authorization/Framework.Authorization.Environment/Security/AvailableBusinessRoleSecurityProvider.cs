using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public class AvailableBusinessRoleSecurityProvider<TDomainObject>(
    IAvailablePermissionSource availablePermissionSource,
    IRelativeDomainPathInfo<TDomainObject, BusinessRole> toBusinessRolePathInfo)
    : SecurityProvider<TDomainObject>
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
