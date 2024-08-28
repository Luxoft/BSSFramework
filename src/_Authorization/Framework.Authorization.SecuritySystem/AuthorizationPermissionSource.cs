using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSource(
    IAvailablePermissionSource availablePermissionSource,
    IRealTypeResolver realTypeResolver,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    ISecurityContextInfoService securityContextInfoService,
    DomainSecurityRule.RoleBaseSecurityRule securityRule) : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = availablePermissionSource.GetAvailablePermissionsQueryable(securityRule)
                                                   .FetchMany(q => q.Restrictions)
                                                   .ThenFetch(q => q.SecurityContextType)
                                                   .ToList();

        return permissions
               .Select(permission => permission.ToDictionary(realTypeResolver, securityContextInfoService, securityTypes))
               .ToList();
    }

    public IQueryable<IPermission> GetPermissionQuery()
    {
        return availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule);
    }

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter)
    {
        var securityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule);

        var visitedFilter = (Expression<Func<Permission, bool>>)AuthVisitor.Visit(permissionFilter);

        return this.GetAccessors(
            visitedFilter,
            new AvailablePermissionFilter(timeProvider.GetToday()) { SecurityRoleIdents = securityRoleIdents.ToList() });
    }



    private IEnumerable<string> GetAccessors(Expression<Func<Permission, bool>> permissionExprFilter, AvailablePermissionFilter availablePermissionFilter)
    {
        if (permissionExprFilter == null) throw new ArgumentNullException(nameof(permissionExprFilter));
        if (availablePermissionFilter == null) throw new ArgumentNullException(nameof(availablePermissionFilter));

        return permissionRepository.GetQueryable()
                                   .Where(availablePermissionFilter.ToFilterExpression())
                                   .Where(permissionExprFilter)
                                   .Select(permission => permission.Principal.Name).ToList();
    }

    private static readonly ExpressionVisitor AuthVisitor = new OverrideParameterTypeVisitor(
        new Dictionary<Type, Type>
        {
            { typeof(IPermission), typeof(Permission) },
            { typeof(IPermissionRestriction), typeof(PermissionRestriction) },
        });
}
