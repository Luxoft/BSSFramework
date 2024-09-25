﻿using Framework.Core;

namespace Framework.SecuritySystem.PermissionOptimization;

public class LegacyRuntimePermissionOptimizationService : IRuntimePermissionOptimizationService
{
    public IEnumerable<Dictionary<Type, List<Guid>>> Optimize(IEnumerable<Dictionary<Type, List<Guid>>> permissions)
    {
        var cachedPermissions = permissions.ToList();

        var groupedPermissionsRequest = from permission in cachedPermissions

                                        let pair = permission.SingleMaybe().GetValueOrDefault()

                                        where !pair.IsDefault()

                                        group permission by pair.Key;

        var groupedPermissions = groupedPermissionsRequest.ToList();

        var aggregatePermissions = groupedPermissions.ToList(pair => new Dictionary<Type, List<Guid>>
        {
                { pair.Key, pair.SelectMany(g => g.Values.Single()).Distinct().ToList() }
        });

        var withoutAggregatePermissions = cachedPermissions.Except(groupedPermissions.SelectMany(g => g));

        return aggregatePermissions.Concat(withoutAggregatePermissions);
    }
}
