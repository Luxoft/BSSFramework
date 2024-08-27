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
            .GetAvailablePermissionsQueryable()
            .Pipe(permissionQ => ExpressionHelper.Create((BusinessRole businessRole) => permissionQ.Select(p => p.Role).Contains(businessRole)))
            .OverrideInput(toBusinessRolePathInfo.Path);

    protected override LambdaCompileMode SecurityFilterCompileMode { get; } = LambdaCompileMode.All;

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var role = toBusinessRolePathInfo.Path.Eval(domainObject);

        return SecurityAccessorData.Return(
            availablePermissionSource.GetAvailablePermissionsQueryable(applyCurrentUser: false)
                .Where(permission => permission.Role == role)
                .Select(permission => permission.Principal)
                .Distinct()
                .Select(principal => principal.Name));
    }
}
