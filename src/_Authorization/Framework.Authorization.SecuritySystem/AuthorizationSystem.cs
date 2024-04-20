using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.PermissionOptimization;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem : IAuthorizationSystem<Guid>
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IRuntimePermissionOptimizationService runtimePermissionOptimizationService;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly IRealTypeResolver realTypeResolver;

    private readonly IOperationAccessorFactory operationAccessorFactory;

    private readonly IRepository<Principal> principalRepository;

    private readonly TimeProvider timeProvider;

    private readonly ISecurityRolesIdentsResolver securityRolesIdentsResolver;

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IRuntimePermissionOptimizationService runtimePermissionOptimizationService,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IRealTypeResolver realTypeResolver,
        IUserAuthenticationService userAuthenticationService,
        IOperationAccessorFactory operationAccessorFactory,
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<Principal> principalRepository,
        ISecurityRuleExpander securityRuleExpander,
        TimeProvider timeProvider,
        ISecurityRolesIdentsResolver securityRolesIdentsResolver)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.runtimePermissionOptimizationService = runtimePermissionOptimizationService;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.realTypeResolver = realTypeResolver;
        this.operationAccessorFactory = operationAccessorFactory;
        this.principalRepository = principalRepository;
        this.timeProvider = timeProvider;
        this.securityRolesIdentsResolver = securityRolesIdentsResolver;

        this.CurrentPrincipalName = userAuthenticationService.GetUserName();
    }

    public string CurrentPrincipalName { get; }

    private IEnumerable<string> GetAccessors(Expression<Func<Principal, bool>> principalFilter, AvailablePermissionFilter permissionFilter)
    {
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));
        if (permissionFilter == null) throw new ArgumentNullException(nameof(permissionFilter));

        var extraPrincipalFilter =
            principalFilter.UpdateBody(
                new OverridePropertyInfoVisitor<Principal, IEnumerable<Permission>>(
                    principal => principal.Permissions, permissionFilter.ToFilterExpression().ToCollectionFilter()));

        var principals = this.principalRepository.GetQueryable().Where(extraPrincipalFilter).ToList();

        return principals.Select(principal => principal.Name);
    }

    public IEnumerable<string> GetNonContextAccessors(
        SecurityRule.DomainObjectSecurityRule securityRule, Expression<Func<IPrincipal<Guid>, bool>> principalFilter)
    {
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

        var securityRoleIdents = this.securityRolesIdentsResolver.Resolve(securityRule);

        return this.GetAccessors(
            (Expression<Func<Principal, bool>>)AuthVisitor.Visit(principalFilter),
            new AvailablePermissionFilter(this.timeProvider.GetToday()) { SecurityRoleIdents = securityRoleIdents.ToList() });
    }

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        SecurityRule.DomainObjectSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var permissions = this.availablePermissionSource.GetAvailablePermissionsQueryable(true, securityRule)
                              .FetchMany(q => q.Restrictions)
                              .ThenFetch(q => q.SecurityContextType)
                              .ToList();
        return permissions
               .Select(permission => permission.ToDictionary(this.realTypeResolver, securityTypes))
               .Pipe(this.runtimePermissionOptimizationService.Optimize)
               .ToList(permission => this.TryExpandPermission(permission, securityRule.CustomExpandType!.Value));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }

    public bool HasAccess(SecurityRule.DomainObjectSecurityRule securityRule) => this.operationAccessorFactory.Create(true).HasAccess(securityRule);

    public void CheckAccess(SecurityRule.DomainObjectSecurityRule securityRule) => this.operationAccessorFactory.Create(true).CheckAccess(securityRule);

    private static readonly ExpressionVisitor AuthVisitor = new OverrideParameterTypeVisitor(
        new Dictionary<Type, Type>
        {
            { typeof(IPrincipal<Guid>), typeof(Principal) },
            { typeof(IPermission<Guid>), typeof(Permission) },
            { typeof(IPermissionRestriction<Guid>), typeof(PermissionRestriction) },
            { typeof(ISecurityContextType<Guid>), typeof(SecurityContextType) },
        });
}
