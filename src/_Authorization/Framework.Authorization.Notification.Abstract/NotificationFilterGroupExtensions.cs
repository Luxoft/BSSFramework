using Framework.Core;

namespace Framework.Authorization.Notification;

public static class NotificationFilterGroupExtensions
{
    private static readonly HashSet<NotificationExpandType> OverrideExpandTypes =

        EnumHelper.GetValues<NotificationExpandType>().Where(expandType => expandType.IsHierarchical()).ToHashSet();


    public static NotificationFilterGroup OverrideExpand(this NotificationFilterGroup group, NotificationExpandType newExpandType)
    {
        return newExpandType == group.ExpandType ? group
                   : new NotificationFilterGroup(group.SecurityContextType, group.Idents, newExpandType);
    }

    public static IEnumerable<NotificationFilterGroup[]> PermuteByExpand(this IEnumerable<NotificationFilterGroup> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var cachedSource = source.ToArray();

        if (cachedSource.Any())
        {
            var authExpandGroups = cachedSource.Where(g => OverrideExpandTypes.Contains(g.ExpandType)).ToArray();

            if (authExpandGroups.Any())
            {
                var request = from currentGroup in authExpandGroups

                              select cachedSource.OrderBy(g => g != currentGroup)
                                                 .ToArray(g => g == currentGroup ? g : g.OverrideExpand(g.ExpandType.WithoutHierarchical()));

                foreach (var permute in request)
                {
                    yield return permute;
                }
            }
            else
            {
                yield return cachedSource;
            }
        }
    }
}
