namespace Framework.Authorization.Domain;

public static class PermissionExtensions
{
    public static IEnumerable<(Guid, Guid)> GetOrderedSecurityContextIdents(this Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.Restrictions.Select(fi => (TypeId: fi.SecurityContextType.Id, fi.SecurityContextId)).OrderBy(id => id.SecurityContextId);
    }
}
