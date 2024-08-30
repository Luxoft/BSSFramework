using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.SecuritySystem;

internal static class ConvertPermissionExtensions
{
    public static IPermission ConvertPermission(this Permission permission, ISecurityContextSource securityContextSource)
    {
        var restrictions =
            permission
                .Restrictions
                .GroupBy(restriction => restriction.SecurityContextType, restriction => restriction.SecurityContextId)
                .ToDictionary(g => securityContextSource.GetSecurityContextInfo(g.Key.Id).Type, g => g.ToReadOnlyListI());

        return new TypedPermission(restrictions, permission.Principal.Name);
    }
}
