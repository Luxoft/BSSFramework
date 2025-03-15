using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.QueryableSource;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    IServiceProvider serviceProvider,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    IUserNameResolver userNameResolver,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    ISecurityContextInfoSource securityContextInfoSource,
    IQueryableSource queryableSource,
    SecurityRuleCredential defaultSecurityRuleCredential)
    : IAvailablePermissionSource
{
    public AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var restrictionFilters = (securityRule.CustomRestriction?.GetSecurityContextRestrictionFilters()).EmptyIfNull();

        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = userNameResolver.Resolve(securityRule.CustomCredential ?? defaultSecurityRuleCredential),
                   SecurityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule).ToList(),
                   RestrictionFilters = restrictionFilters.Select(this.GetRestrictionFilter).ToDictionary()
               };
    }

    private KeyValuePair<Guid, Expression<Func<Guid, bool>>> GetRestrictionFilter(SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        var securityContextTypeId = securityContextInfoSource.GetSecurityContextInfo(restrictionFilterInfo.SecurityContextType).Id;

        var filterExpr = new Func<SecurityContextRestrictionFilterInfo<ISecurityContext>, Expression<Func<Guid, bool>>>(this.GetRestrictionFilterExpression)
                         .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
                         .Invoke<Expression<Func<Guid, bool>>>(this, restrictionFilterInfo);

        return new (securityContextTypeId, filterExpr);
    }

    private Expression<Func<Guid, bool>> GetRestrictionFilterExpression<TSecurityContext>(SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        var filteredSecurityContextQueryable = queryableSource.GetQueryable<TSecurityContext>()
                                                              .Where(restrictionFilterInfo.GetPureFilter(serviceProvider))
                                                              .Select(securityContext => securityContext.Id);

        return securityContextId => filteredSecurityContextQueryable.Contains(securityContextId);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var filter = this.CreateFilter(securityRule);

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
