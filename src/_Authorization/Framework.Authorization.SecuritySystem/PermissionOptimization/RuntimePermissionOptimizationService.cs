using System.Diagnostics.CodeAnalysis;

using Framework.Core;

namespace Framework.Authorization.SecuritySystem.PermissionOptimization;

[SuppressMessage("SonarQube", "S4017", Justification = "Nested type arguments received from interface")]
public class RuntimePermissionOptimizationService : IRuntimePermissionOptimizationService
{
    [SuppressMessage("SonarQube", "S3776", Justification = "Method have high cognitive complexity for performance")]
    public IEnumerable<Dictionary<Type, List<Guid>>> Optimize(IEnumerable<Dictionary<Type, List<Guid>>> permissions)
    {
        var cachedPermissions = permissions.ToList();

        // Data for first grouping probably have maximum length
        // So first grouping keys are created from contexts with minimal data to optimize key comparing
        var orderedTypes = cachedPermissions.SelectMany(p => p)
                                            .GroupBy(p => p.Key)
                                            .Select(g =>
                                                        new { g.Key, Count = g.SelectMany(p => p.Value).Distinct().Count() })
                                            .OrderByDescending(p => p.Count)
                                            .Select(p => p.Key)
                                            .ToList();

        if (orderedTypes.Count == 0)
        {
            return cachedPermissions;
        }

        // Same as cachedPermissions but in this algorithm used HashSet not List as in input parameter.
        // And to avoid conversions from List to HashSet and back used temporary variable.
        // Conversion to HashSet from the begining is not optimal because at least one List will be used as Enumerable.
        IEnumerable<Dictionary<Type, HashSet<Guid>>>? cachedPermissions2 = null;

        foreach (var currentType in orderedTypes)
        {
            // Group permissions by all exising context except one and create Dictionary (for best performance)
            var groupedPermissions =
                GetGroupable(cachedPermissions2, cachedPermissions, currentType)
                    .GroupBy(z => z.Key)
                    .ToDictionary(
                        g => g.Key,
                        gr =>
                        {
                            var values = new HashSet<Guid>();

                            foreach (var currentValues in gr.Select(z => z.Value))
                            {
                                if (currentValues == null)
                                {
                                    return new HashSet<Guid>();
                                }

                                values.UnionWith(currentValues);
                            }

                            return values;
                        });

            // In case of existing permissions with only one context other permissions can be refined
            if (groupedPermissions.ContainsKey(GroupKey.Empty))
            {
                var removeItems = groupedPermissions[GroupKey.Empty];

                // When exists permission without context only this one is necessary
                if (removeItems.Count == 0)
                {
                    return new List<Dictionary<Type, List<Guid>>> { new() };
                }

                groupedPermissions.Remove(GroupKey.Empty);

                var removedKeys = RefineGroupedPermissions(groupedPermissions, removeItems);

                foreach (var key in removedKeys)
                {
                    groupedPermissions.Remove(key);
                }

                groupedPermissions.Add(GroupKey.Empty, removeItems);
            }

            // Back conversion key(few HashSets) and value(single HashSet) to dictionary of HashSets
            // Each groupedPermission equivalent to one permission item and looks like can be converted
            //  to GroupableItem directly. But looks like this conversation is more slow then the current one.
            cachedPermissions2 = groupedPermissions.Select(
                p =>
                    new Dictionary<Type, HashSet<Guid>>(
                        p.Key.GetKeyPairs()
                         .Concat(
                             p.Value.Count > 0
                                 ? new[] { KeyValuePair.Create(currentType, p.Value) }
                                 : Array.Empty<KeyValuePair<Type, HashSet<Guid>>>())));
        }

        return cachedPermissions2?.Select(
                   d =>
                       new Dictionary<Type, List<Guid>>(
                           d.Select(p => KeyValuePair.Create(p.Key, p.Value.ToList()))))
               ?? Array.Empty<Dictionary<Type, List<Guid>>>();
    }

    private static IEnumerable<GroupableItem> GetGroupable(
        IEnumerable<Dictionary<Type, HashSet<Guid>>>? main,
        IEnumerable<Dictionary<Type, List<Guid>>> additional,
        Type currentType) =>
        main?.Select(
            p =>
                new GroupableItem(
                    new GroupKey(p, currentType),
                    p.ContainsKey(currentType) ? p[currentType] : null))
        ?? additional.Select(
            p =>
                new GroupableItem(
                    new GroupKey(p, currentType),
                    p.ContainsKey(currentType) ? p[currentType].ToHashSet() : null));

    [SuppressMessage("SonarQube", "S3242", Justification = "More general type is not necessary here")]
    private static List<GroupKey> RefineGroupedPermissions(
        Dictionary<GroupKey, HashSet<Guid>> groupedPermissions,
        HashSet<Guid> removeItems)
    {
        // This method is executed when exist permission with single current context
        var result = new List<GroupKey>();

        foreach (var pair in groupedPermissions)
        {
            // When permission have no current context, it must be kept as is.
            if (pair.Value.Count == 0)
            {
                continue;
            }

            // Items, which exist in single context permission, must be removed from all other permissions
            pair.Value.ExceptWith(removeItems);

            // In case of all items was removed from current permission, it means that current permission is subset for
            // single context permission. And current permission can be 'optimized' (removed)
            if (pair.Value.Count == 0)
            {
                result.Add(pair.Key);
            }
        }

        return result;
    }

    private record GroupableItem(GroupKey Key, HashSet<Guid>? Value);

    private sealed class GroupKey : IEquatable<GroupKey>
    {
        private readonly int hashCode;

        private readonly IDictionary<Type, HashSet<Guid>> keyData;

        public static readonly GroupKey Empty = new(new Dictionary<Type, List<Guid>>(), typeof(GroupKey));

        public GroupKey(Dictionary<Type, List<Guid>> dataItem, Type excludedType)
        {
            this.keyData = new Dictionary<Type, HashSet<Guid>>(dataItem.Count);
            foreach (var pair in dataItem)
            {
                if (pair.Key == excludedType)
                {
                    continue;
                }

                var hashSet = new HashSet<Guid>(pair.Value.Count);
                for (var i = 0; i < pair.Value.Count; i++)
                {
                    hashSet.Add(pair.Value[i]);
                }

                this.keyData.Add(pair.Key, hashSet);
            }

            this.hashCode = this.CalculateHashCode();
        }

        public GroupKey(Dictionary<Type, HashSet<Guid>> dataItem, Type excludedType)
        {
            this.keyData = new Dictionary<Type, HashSet<Guid>>(dataItem.Where(pair => pair.Key != excludedType));
            this.hashCode = this.CalculateHashCode();
        }

        public IEnumerable<KeyValuePair<Type, HashSet<Guid>>> GetKeyPairs() => this.keyData;

        public override int GetHashCode() => this.hashCode;

        public bool Equals(GroupKey? other) => this.Equals((object?)other);

        public override bool Equals(object? obj) =>
            !ReferenceEquals(null, obj)
            && (ReferenceEquals(this, obj)
                || (obj.GetType() == this.GetType() && this.DataEquals((GroupKey)obj)));

        private int CalculateHashCode()
        {
            var result = 0;
            foreach (var pair in this.keyData)
            {
                result ^= pair.Key.GetHashCode();
                foreach (var val in pair.Value)
                {
                    result ^= val.GetHashCode();
                }
            }

            return result;
        }

        private bool DataEquals(GroupKey other)
        {
            if (this.keyData.Count != other.keyData.Count)
            {
                return false;
            }

            foreach (var pair in this.keyData)
            {
                if (!other.keyData.ContainsKey(pair.Key))
                {
                    return false;
                }

                var otherKeyData = other.keyData[pair.Key];
                if (otherKeyData.Count != pair.Value.Count)
                {
                    return false;
                }

                foreach (var id in pair.Value)
                {
                    if (!otherKeyData.Contains(id))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
