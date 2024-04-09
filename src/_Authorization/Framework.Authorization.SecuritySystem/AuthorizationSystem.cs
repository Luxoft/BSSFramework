using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.PermissionOptimization;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

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

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IRuntimePermissionOptimizationService runtimePermissionOptimizationService,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IRealTypeResolver realTypeResolver,
        IUserAuthenticationService userAuthenticationService,
        IOperationAccessorFactory operationAccessorFactory,
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<Principal> principalRepository,
        TimeProvider timeProvider)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.runtimePermissionOptimizationService = runtimePermissionOptimizationService;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.realTypeResolver = realTypeResolver;
        this.operationAccessorFactory = operationAccessorFactory;
        this.principalRepository = principalRepository;
        this.timeProvider = timeProvider;

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
        SecurityRule securityRule, Expression<Func<IPrincipal<Guid>, bool>> principalFilter)
    {
        if (principalFilter == null) throw new ArgumentNullException(nameof(principalFilter));

        var typedSecurityOperation = (SecurityRule)securityRule;

        return this.GetAccessors(
            (Expression<Func<Principal, bool>>)AuthVisitor.Visit(principalFilter),
            new AvailablePermissionFilter(this.timeProvider.GetToday()) { SecurityOperationId = typedSecurityOperation.Id });
    }

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        SecurityRule.DomainObjectSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var permissions = this.availablePermissionSource.GetAvailablePermissionsQueryable(true, securityRule.Id)
                              .FetchMany(q => q.FilterItems)
                              .ThenFetch(q => q.Entity)
                              .ThenFetch(q => q.EntityType)
                              .ToList();

        var securityTypesCache = securityTypes.ToReadOnlyCollection();

        return permissions
               .Select(permission => permission.ToDictionary(this.realTypeResolver, securityTypesCache))
               .Pipe(this.runtimePermissionOptimizationService.Optimize)
               .ToList(permission => this.TryExpandPermission(permission, securityRule.ExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        var typedSecurityOperation = (SecurityRule)securityRule;

        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityOperationId: typedSecurityOperation.Id);
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

    public bool IsAdmin() => this.operationAccessorFactory.Create(true).IsAdmin();

    public bool HasAccess(SecurityRule securityRule) => this.operationAccessorFactory.Create(true).HasAccess(securityRule);

    public void CheckAccess(SecurityRule securityRule) => this.operationAccessorFactory.Create(true).CheckAccess(securityRule);

    private static readonly ExpressionVisitor AuthVisitor = new OverrideParameterTypeVisitor(
        new Dictionary<Type, Type>
        {
            { typeof(IPrincipal<Guid>), typeof(Principal) },
            { typeof(IPermission<Guid>), typeof(Permission) },
            { typeof(IPermissionFilterItem<Guid>), typeof(PermissionFilterItem) },
            { typeof(IPermissionFilterEntity<Guid>), typeof(PermissionFilterEntity) },
            { typeof(IEntityType<Guid>), typeof(EntityType) },
        });
}
