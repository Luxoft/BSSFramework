using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem : IRunAsAuthorizationSystem
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IAuthOperationResolver authOperationResolver;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    private readonly IDateTimeService dateTimeService;

    private readonly IRunAsManager runAsManager;

    private readonly IRepositoryFactory<Permission> permissionRepositoryFactory;

    private readonly IRuntimePermissionOptimizationService runtimePermissionOptimizationService;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly IRealTypeResolver realTypeResolver;

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IAuthOperationResolver authOperationResolver,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        IDateTimeService dateTimeService,
        IRunAsManager runAsManager,
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IRuntimePermissionOptimizationService runtimePermissionOptimizationService,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IRealTypeResolver realTypeResolver)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.authOperationResolver = authOperationResolver;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
        this.dateTimeService = dateTimeService;
        this.runAsManager = runAsManager;
        this.permissionRepositoryFactory = permissionRepositoryFactory;
        this.runtimePermissionOptimizationService = runtimePermissionOptimizationService;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.realTypeResolver = realTypeResolver;
    }

    public bool IsAdmin() => this.availablePermissionSource.GetAvailablePermissionsQueryable()
                                 .Any(permission => permission.Role.Name == BusinessRole.AdminRoleName);

    public bool HasAccess(NonContextSecurityOperation securityOperation)
    {
        var authOperation = this.authOperationResolver.GetAuthOperation(securityOperation);

        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityOperationId: authOperation.Id).Any();
    }

    public void CheckAccess(NonContextSecurityOperation operation)
    {
        if (!this.HasAccess(operation))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(new AccessResult.AccessDeniedResult { SecurityOperation = operation });
        }
    }

    public IEnumerable<string> GetAccessors(NonContextSecurityOperation securityOperation, Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(ContextSecurityOperation securityOperation, IEnumerable<Type> securityTypes)
    {
        var typedSecurityOperation = (ContextSecurityOperation<Guid>)securityOperation;

        var filter = new AvailablePermissionFilter(this.dateTimeService.Today)
                     {
                         PrincipalName = this.runAsManager.PrincipalName,
                         SecurityOperationId = typedSecurityOperation.Id
                     };

        var permissions = this.permissionRepositoryFactory.Create().GetQueryable().Where(filter.ToFilterExpression())
                              .FetchMany(q => q.FilterItems)
                              .ThenFetch(q => q.Entity)
                              .ThenFetch(q => q.EntityType)
                              .ToList();

        var securityTypesCache = securityTypes.ToReadOnlyCollection();

        return permissions
               .Select(permission => permission.ToDictionary(this.realTypeResolver, securityTypesCache))
               .Pipe(this.runtimePermissionOptimizationService.Optimize)
               .ToList(permission => this.TryExpandPermission(permission, securityOperation.ExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(ContextSecurityOperation securityOperation) => throw new NotImplementedException();



    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(Dictionary<Type, List<Guid>> permission, HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
