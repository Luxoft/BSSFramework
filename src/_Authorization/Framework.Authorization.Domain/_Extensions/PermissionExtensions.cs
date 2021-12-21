using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Projection;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using DPermission = System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<System.Guid>>;

namespace Framework.Authorization.Domain
{
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

        public static IEnumerable<DPermission> Optimize(this IEnumerable<DPermission> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            var cachedPermissions = permissions.ToList();

            var groupedPermissionsRequest = from permission in cachedPermissions

                                            let pair = permission.SingleMaybe().GetValueOrDefault()

                                            where !pair.IsDefault()

                                            group permission by pair.Key;

            var groupedPermissions = groupedPermissionsRequest.ToList();

            var aggregatePermissions = groupedPermissions.ToList(pair => new DPermission
            {
                { pair.Key, pair.SelectMany(g => g.Values.Single()).Distinct().ToList() }
            });

            var withoutAggregatePermissions = cachedPermissions.Except(groupedPermissions.SelectMany(g => g));

            return aggregatePermissions.Concat(withoutAggregatePermissions);
        }
    }
}
