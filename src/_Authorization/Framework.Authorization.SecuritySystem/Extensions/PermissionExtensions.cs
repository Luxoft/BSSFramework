using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using DPermission = System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<System.Guid>>;

namespace Framework.Authorization.SecuritySystem;

public static class PermissionExtensions
{
    public static DPermission ToDictionary(
        this IPermission<Guid> permission,
        IRealTypeResolver realTypeResolver,
        ISecurityContextInfoService<Guid> securityContextInfoService,
        IEnumerable<Type> securityTypes)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var request = from restriction in permission.Restrictions

                      join securityType in securityTypes on restriction.SecurityContextTypeId equals securityContextInfoService
                          .GetSecurityContextInfo(realTypeResolver.Resolve(securityType)).Id

                      group restriction.SecurityContextId by securityType;

        return request.ToDictionary(g => g.Key, g => g.ToList());
    }
}
