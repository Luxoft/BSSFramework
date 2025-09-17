using System.Linq.Expressions;

using CommonFramework;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.Services;

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

            from securityContextRestriction in securityRule.GetSafeSecurityContextRestrictions()

            where securityContextRestriction.RawFilter != null

            let securityContextType = securityContextInfoSource.GetSecurityContextInfo(securityContextRestriction.SecurityContextType)

            let filter = this.GetRestrictionFilter(securityContextRestriction.RawFilter)

            select (securityContextType.Id, (!securityContextRestriction.Required, filter));


        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = userNameResolver.Resolve(securityRule.CustomCredential ?? defaultSecurityRuleCredential),
                   SecurityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule).ToList(),
                   RestrictionFilters = restrictionFiltersRequest.ToDictionary()
               };
    }

    private Expression<Func<Guid, bool>> GetRestrictionFilter(SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        return new Func<SecurityContextRestrictionFilterInfo<ISecurityContext>, Expression<Func<Guid, bool>>>(this.GetRestrictionFilterExpression)
               .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
               .Invoke<Expression<Func<Guid, bool>>>(this, restrictionFilterInfo);
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
