using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class RunAsDisabledOperationAccessor : OperationAccessorBase, IRunAsDisabledOperationAccessor
{
    public RunAsDisabledOperationAccessor(IAvailablePermissionSource availablePermissionSource, IAccessDeniedExceptionService accessDeniedExceptionService)
        : base(availablePermissionSource, accessDeniedExceptionService, true)
    {
    }
}
