using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSource(
    IAvailablePermissionSource availablePermissionSource,
    IRealTypeResolver realTypeResolver,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    ISecurityContextInfoSource securityContextInfoSource,
    ISecurityContextSource securityContextSource,
    DomainSecurityRule.RoleBaseSecurityRule securityRule) : IPermissionSource<Permission>
{
    public bool HasAccess()
    {
        return this.GetPermissionQuery().Any();
    }

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityContextTypes)
    {
        var permissions = availablePermissionSource.GetAvailablePermissionsQueryable(securityRule)
                                                   .WithFetch($"{nameof(Permission.Restrictions)}.{nameof(PermissionRestriction.SecurityContextType)}")
                                                   //.FetchMany(q => q.Restrictions)
                                                   //.ThenFetch(q => q.SecurityContextType)
                                                   .ToList();

        return permissions
               .Select(permission => this.ConvertPermission(permission, securityContextTypes))
               .ToList();
    }

    public IQueryable<Permission> GetPermissionQuery()
    {
        return this.GetSecurityPermissions(availablePermissionSource.CreateFilter(securityRule: securityRule));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<Permission, bool>> permissionFilter)
    {
        return this.GetSecurityPermissions(
                       availablePermissionSource.CreateFilter(securityRule with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() }))
                   .Where(permissionFilter)
                   .Select(permission => permission.Principal.Name);
    }

    private IQueryable<Permission> GetSecurityPermissions(AvailablePermissionFilter availablePermissionFilter)
    {
        return permissionRepository.GetQueryable()
                                   .Where(availablePermissionFilter.ToFilterExpression());
    }

    private Dictionary<Type, List<Guid>> ConvertPermission(
        Permission permission,
        IEnumerable<Type> securityContextTypes)
    {
        var purePermission = permission.Restrictions.GroupBy(
                                           permissionRestriction => permissionRestriction.SecurityContextType.Id,
                                           permissionRestriction => permissionRestriction.SecurityContextId)
                                       .ToDictionary(g => g.Key, g => g.ToList());

        var filterInfoDict = securityRule.GetSafeSecurityContextRestrictionFilters().ToDictionary(filterInfo => filterInfo.SecurityContextType);

        return securityContextTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType =>
            {
                var securityContextRestrictionFilterInfo = filterInfoDict.GetValueOrDefault(securityContextType);

                var securityContextTypeId =
                    securityContextInfoSource.GetSecurityContextInfo(realTypeResolver.Resolve(securityContextType)).Id;

                var baseIdents = purePermission.GetValueOrDefault(securityContextTypeId, []);

                if (securityContextRestrictionFilterInfo == null)
                {
                    return baseIdents;
                }
                else
                {
                    return this.ApplySecurityContextFilter(baseIdents, securityContextRestrictionFilterInfo);
                }

            });
    }

    private List<Guid> ApplySecurityContextFilter(List<Guid> securityContextIdents, SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        return new Func<List<Guid>, SecurityContextRestrictionFilterInfo<ISecurityContext>, List<Guid>>(this.ApplySecurityContextFilter)
               .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
               .Invoke<List<Guid>>(this, securityContextIdents, restrictionFilterInfo);
    }

    private List<Guid> ApplySecurityContextFilter<TSecurityContext>(List<Guid> baseSecurityContextIdents, SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        var filteredSecurityContextQueryable = securityContextSource.GetQueryable(restrictionFilterInfo).Select(securityContext => securityContext.Id);

        if (baseSecurityContextIdents.Any())
        {
            return filteredSecurityContextQueryable.Where(securityContextId => baseSecurityContextIdents.Contains(securityContextId))
                                                   .ToList();
        }
        else
        {
            return filteredSecurityContextQueryable.ToList();
        }
    }
}
