using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public class AvailableBusinessRoleSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>

    where TDomainObject : PersistentDomainObjectBase, IIdentityObject<Guid>
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly Expression<Func<TDomainObject, BusinessRole>> businessRoleSecurityPath;

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> lazySecurityFilter;

    public AvailableBusinessRoleSecurityProvider(
        IAvailablePermissionSource availablePermissionSource,
        IRelativeDomainPathInfo<TDomainObject, BusinessRole> toBusinessRolePathInfo)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.businessRoleSecurityPath = toBusinessRolePathInfo.Path;

        this.lazySecurityFilter = LazyHelper.Create(
            () =>
            {
                var permissionQ = availablePermissionSource.GetAvailablePermissionsQueryable();

                Expression<Func<BusinessRole, bool>> filter = businessRole => permissionQ.Select(p => p.Role).Contains(businessRole);

                return filter.OverrideInput(this.businessRoleSecurityPath);
            });
    }

    protected override LambdaCompileMode SecurityFilterCompileMode { get; } = LambdaCompileMode.All;

    public override Expression<Func<TDomainObject, bool>> SecurityFilter => this.lazySecurityFilter.Value;

    public override SecurityAccessorResult GetAccessors(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var role = this.businessRoleSecurityPath.Eval(domainObject);

        return SecurityAccessorResult.Return(
            this.availablePermissionSource.GetAvailablePermissionsQueryable(applyCurrentUser: false)
                .Where(permission => permission.Role == role)
                .Select(permission => permission.Principal)
                .Distinct()
                .Select(principal => principal.Name));
    }
}
