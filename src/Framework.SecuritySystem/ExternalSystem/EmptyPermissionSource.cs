using System.Linq.Expressions;

namespace Framework.SecuritySystem.ExternalSystem;

public class EmptyPermissionSource : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes) => [];

    public IQueryable<IPermission> GetPermissionQuery() => Enumerable.Empty<IPermission>().AsQueryable();

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter) => [];
}
