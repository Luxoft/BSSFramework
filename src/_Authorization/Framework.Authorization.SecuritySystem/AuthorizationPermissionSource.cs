using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.QueryableSource;
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

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = availablePermissionSource.GetAvailablePermissionsQueryable(securityRule)
                                                   //.FetchMany(q => q.Restrictions)
                                                   //.ThenFetch(q => q.SecurityContextType)
                                                   .ToList();

        return permissions
               .Select(permission => this.ConvertPermission(permission, securityTypes))
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
        var request = from restriction in permission.Restrictions

                      join securityContextType in securityContextTypes on restriction.SecurityContextType.Id equals

                          securityContextInfoSource.GetSecurityContextInfo(realTypeResolver.Resolve(securityContextType)).Id

                      group restriction.SecurityContextId by securityContextType

                      into restrictionGroup

                      join securityContextRestrictionFilter in securityRule.GetSafeSecurityContextRestrictionFilters() on

                          restrictionGroup.Key equals securityContextRestrictionFilter.SecurityContextType into filterGroup

                      let securityContextRestrictionFilterInfo = filterGroup.SingleOrDefault()

                      let filteredSecurityContextIdents =

                          securityContextRestrictionFilterInfo == null
                              ? restrictionGroup.ToList()
                              : this.ApplySecurityContextFilter(restrictionGroup, securityContextRestrictionFilterInfo)

                      where filteredSecurityContextIdents.Any()

                      select (restrictionGroup.Key, filteredSecurityContextIdents);

        return request.ToDictionary();
    }

    private List<Guid> ApplySecurityContextFilter(IEnumerable<Guid> securityContextIdents, SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        return new Func<IEnumerable<Guid>, SecurityContextRestrictionFilterInfo<ISecurityContext>, IEnumerable<Guid>>(
                   this.ApplySecurityContextFilter)
               .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
               .Invoke<List<Guid>>(this, securityContextIdents, restrictionFilterInfo);
    }

    private List<Guid> ApplySecurityContextFilter<TSecurityContext>(IEnumerable<Guid> securityContextIdents, SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        return securityContextSource.GetQueryable(restrictionFilterInfo)
                                    .Where(securityContext => securityContextIdents.Contains(securityContext.Id))
                                    .Select(securityContext => securityContext.Id)
                                    .ToList();
    }
}
