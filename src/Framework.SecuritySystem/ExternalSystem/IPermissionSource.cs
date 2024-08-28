using System.Linq.Expressions;

namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSource
{
    List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes);

    IQueryable<IPermission> GetPermissionQuery();

    IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter);
}
