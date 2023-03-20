using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Projection;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using DPermission = System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<System.Guid>>;

namespace Framework.Authorization.Domain;

public static class PermissionExtensions
{
    public static IEnumerable<Guid> GetOrderedEntityIdents([NotNull] this Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.FilterItems.Select(fi => fi.Entity.EntityId).OrderBy(id => id);
    }

    public static DPermission ToDictionary(this IPermission<Guid> permission, IEnumerable<Type> securityTypes)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var request = from filterItem in permission.FilterItems

                      join securityType in securityTypes on filterItem.Entity.EntityType.Name equals securityType.GetProjectionSourceTypeOrSelf().Name

                      group filterItem.Entity.EntityId by securityType;

        return request.ToDictionary(g => g.Key, g => g.ToList());
    }
}
