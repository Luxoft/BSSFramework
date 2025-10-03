using Framework.Core;

using SecuritySystem.ExternalSystem.Management;

namespace Framework.Authorization.SecuritySystemImpl;

public static class TypedPermissionExtensions
{
    public static Period GetPeriod(this TypedPermission typedPermission)
    {
        return new Period(typedPermission.StartDate, typedPermission.EndDate);
    }
}
