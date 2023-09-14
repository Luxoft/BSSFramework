﻿using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystem : IAuthorizationSystem<Guid>
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IOperationResolver operationResolver;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    private readonly IRepositoryFactory<EntityType> repositoryFactory;

    public AuthorizationSystem(
        IAvailablePermissionSource availablePermissionSource,
        IOperationResolver operationResolver,
        IAccessDeniedExceptionService accessDeniedExceptionService)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.operationResolver = operationResolver;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
    }

    public bool IsAdmin() => this.availablePermissionSource.GetAvailablePermissionsQueryable()
                                 .Any(permission => permission.Role.Name == BusinessRole.AdminRoleName);

    public bool HasAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
        where TSecurityOperationCode : struct, Enum
    {
        var authOperation = this.operationResolver.GetByCode(securityOperation.Code);

        return this.availablePermissionSource.GetAvailablePermissionsQueryable()
                   .Any(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation == authOperation));
    }

    public void CheckAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> operation)
        where TSecurityOperationCode : struct, Enum
    {
        if (!this.HasAccess(operation))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(new AccessResult.AccessDeniedResult { SecurityOperation = operation });
        }
    }

    public IEnumerable<string> GetAccessors<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode, Expression<Func<IPrincipal<Guid>, bool>> principalFilter)
        where TSecurityOperationCode : struct, Enum =>
        throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation, IEnumerable<Type> securityTypes)
        where TSecurityOperationCode : struct, Enum =>
        throw new NotImplementedException();

    public IQueryable<IPermission<Guid>> GetPermissionQuery<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
        where TSecurityOperationCode : struct, Enum =>
        throw new NotImplementedException();
}

public class AvailablePermissionSpecification<TSecurityOperationCode> : AvailablePermissionFilter
    where TSecurityOperationCode : struct, Enum
{
    private readonly Guid securityOperationId;

    public AvailablePermissionOperationFilter(IDateTimeService dateTimeService, string principalName, TSecurityOperationCode securityOperationCode)
        : base(dateTimeService, principalName)
    {
        if (securityOperationCode.IsDefault()) throw new ArgumentOutOfRangeException(nameof(securityOperationCode));

        this.securityOperationId = securityOperationCode.ToGuid();
    }

    public override Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return base.ToFilterExpression()
                   .BuildAnd(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation.Id == this.securityOperationId));
    }
}