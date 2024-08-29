using System.Linq.Expressions;

namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionSource
{
    List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes);

    IQueryable<IPermission> GetPermissionQuery();

    Expression<Func<IPermission, IEnumerable<Guid>>> GetPermissionRestrictionExpression<TSecurityContext>()
        where TSecurityContext : ISecurityContext;

    IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter);
}
