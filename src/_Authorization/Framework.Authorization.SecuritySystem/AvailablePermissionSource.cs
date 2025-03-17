using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    IUserNameResolver userNameResolver,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    ISecurityContextInfoSource securityContextInfoSource,
    ISecurityContextSource securityContextSource,
    SecurityRuleCredential defaultSecurityRuleCredential)
    : IAvailablePermissionSource
{
    public AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var restrictionFiltersRequest =

            from securityContextRestriction in (securityRule.CustomRestriction?.SecurityContextRestrictions).EmptyIfNull()

            where securityContextRestriction.RawFilter != null

            let filterData = this.GetRestrictionFilter(securityContextRestriction.RawFilter)

            select (filterData.Key, (!securityContextRestriction.Required, filterData.Value));


        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = userNameResolver.Resolve(securityRule.CustomCredential ?? defaultSecurityRuleCredential),
                   SecurityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule).ToList(),
                   RestrictionFilters = restrictionFiltersRequest.ToDictionary()
               };
    }

    private KeyValuePair<Guid, Expression<Func<Guid, bool>>> GetRestrictionFilter(SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        var securityContextTypeId = securityContextInfoSource.GetSecurityContextInfo(restrictionFilterInfo.SecurityContextType).Id;

        var filterExpr = new Func<SecurityContextRestrictionFilterInfo<ISecurityContext>, Expression<Func<Guid, bool>>>(this.GetRestrictionFilterExpression)
                         .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
                         .Invoke<Expression<Func<Guid, bool>>>(this, restrictionFilterInfo);

        return new(securityContextTypeId, filterExpr);
    }

    private Expression<Func<Guid, bool>> GetRestrictionFilterExpression<TSecurityContext>(
        SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        var filteredSecurityContextQueryable = securityContextSource.GetQueryable(restrictionFilterInfo)
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
