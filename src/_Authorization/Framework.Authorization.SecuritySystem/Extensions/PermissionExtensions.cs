using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using DPermission = System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<System.Guid>>;

namespace Framework.Authorization.SecuritySystem;

public static class PermissionExtensions
{
    public static DPermission ToDictionary(this IPermission<Guid> permission, IRealTypeResolver realTypeResolver, IEnumerable<Type> securityTypes)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var request = from filterItem in permission.Restrictions

                      join securityType in securityTypes on filterItem.SecurityContextType.Name equals realTypeResolver.Resolve(securityType).Name

                      group filterItem.SecurityContextId by securityType;

        return request.ToDictionary(g => g.Key, g => g.ToList());
    }
}
