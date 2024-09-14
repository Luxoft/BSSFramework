using System.Linq.Expressions;

namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSource
{
    bool HasAccess();

    List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes);
}

public interface IPermissionSource<TPermission> : IPermissionSource
{
    IQueryable<TPermission> GetPermissionQuery();

    IEnumerable<string> GetAccessors(Expression<Func<TPermission, bool>> permissionFilter);
}
