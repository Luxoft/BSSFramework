using System.Linq.Expressions;

using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSource(ExamplePermissionSystemData data) : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes) => data.Permissions;

    public IQueryable<IPermission> GetPermissionQuery() => throw new NotImplementedException();

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter) => throw new NotImplementedException();
}
