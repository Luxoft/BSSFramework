using System.Linq.Expressions;

using Framework.Authorization.Domain;
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
    ISecurityContextSource securityContextSource,
    DomainSecurityRule.RoleBaseSecurityRule securityRule) : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = availablePermissionSource.GetAvailablePermissionsQueryable(securityRule)
                                                   .FetchMany(q => q.Restrictions)
                                                   .ThenFetch(q => q.SecurityContextType)
                                                   .ToList();

        return permissions
               .Select(permission => permission.ToDictionary(realTypeResolver, securityContextSource, securityTypes))
               .ToList();
    }

    public IQueryable<IPermission> GetPermissionQuery()
    {
        return this.GetSecurityPermissions(availablePermissionSource.CreateFilter(securityRule: securityRule));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter)
    {
        return this.GetSecurityPermissions(availablePermissionSource.CreateFilter(securityRule, applyCurrentUser: false))
                   .Where(permissionFilter)
                   .Select(permission => permission.PrincipalName);
    }

    private IQueryable<IPermission> GetSecurityPermissions(AvailablePermissionFilter availablePermissionFilter)
    {
        return permissionRepository.GetQueryable()
                                   .Where(availablePermissionFilter.ToFilterExpression())
                                   .Select(permission => permission.ConvertPermission(securityContextSource));
    }
}
