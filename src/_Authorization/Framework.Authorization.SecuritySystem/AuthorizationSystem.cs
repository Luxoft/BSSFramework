using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.Services;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem : IAuthorizationSystem<Guid>
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    private readonly IRuntimePermissionOptimizationService runtimePermissionOptimizationService;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly IRealTypeResolver realTypeResolver;

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        IRuntimePermissionOptimizationService runtimePermissionOptimizationService,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IRealTypeResolver realTypeResolver,
        IUserAuthenticationService userAuthenticationService)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
        this.runtimePermissionOptimizationService = runtimePermissionOptimizationService;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.realTypeResolver = realTypeResolver;

        this.CurrentPrincipalName = userAuthenticationService.GetUserName();
    }

    public string CurrentPrincipalName { get; }


    public IEnumerable<string> GetAccessors(
        NonContextSecurityOperation securityOperation,
        Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        ContextSecurityOperation securityOperation,
        IEnumerable<Type> securityTypes)
    {
        var typedSecurityOperation = (ContextSecurityOperation<Guid>)securityOperation;

        var permissions = this.availablePermissionSource.GetAvailablePermissionsQueryable(true, typedSecurityOperation.Id)
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

    public IQueryable<IPermission<Guid>> GetPermissionQuery(ContextSecurityOperation securityOperation) =>
        throw new NotImplementedException();



    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
