using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class OperationAccessorFactory : IOperationAccessorFactory
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    public OperationAccessorFactory(IAvailablePermissionSource availablePermissionSource, IAccessDeniedExceptionService accessDeniedExceptionService)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
    }

    public IOperationAccessor Create(bool withRunAs)
    {
        return new OperationAccessor(this.availablePermissionSource, this.accessDeniedExceptionService, withRunAs);
    }
}
