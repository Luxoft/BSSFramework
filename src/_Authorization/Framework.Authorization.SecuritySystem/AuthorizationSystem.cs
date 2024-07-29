using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.PermissionOptimization;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem(
    IAvailablePermissionSource availablePermissionSource,
    IRuntimePermissionOptimizationService runtimePermissionOptimizationService,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    IRealTypeResolver realTypeResolver,
    IUserAuthenticationService userAuthenticationService,
    IOperationAccessorFactory operationAccessorFactory,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    ISecurityContextInfoService securityContextInfoService)
    : IAuthorizationSystem<Guid>
{
    public string CurrentPrincipalName { get; } = userAuthenticationService.GetUserName();

    private IEnumerable<string> GetAccessors(Expression<Func<Permission, bool>> permissionExprFilter, AvailablePermissionFilter availablePermissionFilter)
    {
        if (permissionExprFilter == null) throw new ArgumentNullException(nameof(permissionExprFilter));
        if (availablePermissionFilter == null) throw new ArgumentNullException(nameof(availablePermissionFilter));

        return permissionRepository.GetQueryable()
                                   .Where(availablePermissionFilter.ToFilterExpression())
                                   .Where(permissionExprFilter)
                                   .Select(permission => permission.Principal.Name).ToList();
    }

    public IEnumerable<string> GetNonContextAccessors(
        DomainSecurityRule.RoleBaseSecurityRule securityRule, Expression<Func<IPermission<Guid>, bool>> permissionFilter)
    {
        if (permissionFilter == null) throw new ArgumentNullException(nameof(permissionFilter));

        var securityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule);

        var visitedFilter = (Expression<Func<Permission, bool>>)AuthVisitor.Visit(permissionFilter);

        return this.GetAccessors(
            visitedFilter,
            new AvailablePermissionFilter(timeProvider.GetToday()) { SecurityRoleIdents = securityRoleIdents.ToList() });
    }

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var permissions = availablePermissionSource.GetAvailablePermissionsQueryable(true, securityRule)
                              .FetchMany(q => q.Restrictions)
                              .ThenFetch(q => q.SecurityContextType)
                              .ToList();
        return permissions
               .Select(permission => permission.ToDictionary(realTypeResolver, securityContextInfoService, securityTypes))
               .Pipe(runtimePermissionOptimizationService.Optimize)
               .ToList(permission => this.TryExpandPermission(permission, securityRule.SafeExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => operationAccessorFactory.Create(true).HasAccess(securityRule);

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => operationAccessorFactory.Create(true).CheckAccess(securityRule);

    private static readonly ExpressionVisitor AuthVisitor = new OverrideParameterTypeVisitor(
        new Dictionary<Type, Type>
        {
            { typeof(IPermission<Guid>), typeof(Permission) },
            { typeof(IPermissionRestriction<Guid>), typeof(PermissionRestriction) },
        });
}
