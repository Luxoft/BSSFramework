namespace Framework.Authorization.Domain;

public static class PermissionExtensions
{
    public static IEnumerable<Guid> GetOrderedSecurityContextIdents(this Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.Restrictions.Select(fi => fi.SecurityContextId).OrderBy(id => id);
    }
}
