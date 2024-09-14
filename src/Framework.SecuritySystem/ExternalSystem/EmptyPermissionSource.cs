using System.Linq.Expressions;

namespace Framework.SecuritySystem.ExternalSystem;

public class EmptyPermissionSource<TPermission> : IPermissionSource<TPermission>
{
    public bool HasAccess() => false;

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> _) => [];

    public IQueryable<TPermission> GetPermissionQuery() => Enumerable.Empty<TPermission>().AsQueryable();

    public IEnumerable<string> GetAccessors(Expression<Func<TPermission, bool>> _) => [];
}
