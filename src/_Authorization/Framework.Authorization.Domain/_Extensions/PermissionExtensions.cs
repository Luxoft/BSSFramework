namespace Framework.Authorization.Domain;

public static class PermissionExtensions
{
    public static IEnumerable<Guid> GetOrderedEntityIdents(this Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.FilterItems.Select(fi => fi.Entity.EntityId).OrderBy(id => id);
    }
}
