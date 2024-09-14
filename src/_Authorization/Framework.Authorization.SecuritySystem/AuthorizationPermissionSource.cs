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
    DomainSecurityRule.RoleBaseSecurityRule securityRule) : IPermissionSource<Permission>
{
    public bool HasAccess()
    {
        return this.GetPermissionQuery().Any();
    }

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

    public IQueryable<Permission> GetPermissionQuery()
    {
        return this.GetSecurityPermissions(availablePermissionSource.CreateFilter(securityRule: securityRule));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<Permission, bool>> permissionFilter)
    {
        return this.GetSecurityPermissions(availablePermissionSource.CreateFilter(securityRule))
                   .Where(permissionFilter)
                   .Select(permission => permission.Principal.Name);
    }

    private IQueryable<Permission> GetSecurityPermissions(AvailablePermissionFilter availablePermissionFilter)
    {
        return permissionRepository.GetQueryable()
                                   .Where(availablePermissionFilter.ToFilterExpression());
    }
}
