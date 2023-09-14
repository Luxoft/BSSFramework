using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem : IAuthorizationSystem<Guid>
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IAuthOperationResolver authOperationResolver;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    private readonly IRepositoryFactory<EntityType> repositoryFactory;

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IAuthOperationResolver authOperationResolver,
        IAccessDeniedExceptionService accessDeniedExceptionService)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.authOperationResolver = authOperationResolver;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
    }

    public bool IsAdmin() => this.availablePermissionSource.GetAvailablePermissionsQueryable()
                                 .Any(permission => permission.Role.Name == BusinessRole.AdminRoleName);

    public bool HasAccess(NonContextSecurityOperation securityOperation)
    {
        var authOperation = this.authOperationResolver.GetAuthOperation(securityOperation);

        return this.availablePermissionSource.GetAvailablePermissionsQueryable(operationId: authOperation.Id)
                   .Any(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation == authOperation));
    }

    public void CheckAccess(NonContextSecurityOperation operation)
    {
        if (!this.HasAccess(operation))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(new AccessResult.AccessDeniedResult { SecurityOperation = operation });
        }
    }

    public IEnumerable<string> GetAccessors(NonContextSecurityOperation securityOperation, Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(ContextSecurityOperation securityOperation, IEnumerable<Type> securityTypes) => throw new NotImplementedException();

    public IQueryable<IPermission<Guid>> GetPermissionQuery(ContextSecurityOperation securityOperation) => throw new NotImplementedException();
}
