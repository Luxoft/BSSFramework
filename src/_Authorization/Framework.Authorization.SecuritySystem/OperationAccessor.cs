using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class OperationAccessor : OperationAccessorBase, IOperationAccessor
{
    public OperationAccessor(IAvailablePermissionSource availablePermissionSource, IAccessDeniedExceptionService accessDeniedExceptionService)
        : base(availablePermissionSource, accessDeniedExceptionService, true)
    {
    }
}
