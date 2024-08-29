using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSource<TDomainObject>(ISecurityContextSource securityContextSource, ICurrentUser currentUser) : IPermissionSource
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = this.GetPermissionQuery().ToList();

        return permissions.Select(this.ConvertPermission).ToList();

    }

    public IQueryable<IPermission> GetPermissionQuery(bool applyCurrentUser) =>
        this.GetPermissionQuery().Pipe(applyCurrentUser, q => q.Where(p => p.PrincipalName == currentUser.Name));

    public IQueryable<IPermission> GetPermissionQuery() => throw new NotImplementedException();

    public IEnumerable<string> GetAccessors(Expression<Func<IPermission, bool>> permissionFilter) =>
        this.GetPermissionQuery().Where(permissionFilter).Select(p => p.PrincipalName);

    private Dictionary<Type, List<Guid>> ConvertPermission(IPermission permission) =>
        permission
            .Restrictions
            .GroupBy(p => p.SecurityContextTypeId)
            .Select(g => (securityContextSource.GetSecurityContextInfo(g.Key).Type, g.Select(v => v.SecurityContextId).ToList()))
            .ToDictionary();
}
