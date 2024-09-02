using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSource<TDomainObject>(
    ICurrentUser currentUser,
    IQueryableSource queryableSource,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermissionSource<TDomainObject>
{
    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = this.GetPermissionQuery(true).ToList();

        return permissions.Select(permission => this.ConvertPermission(permission, securityTypes)).ToList();
    }

    public IQueryable<TDomainObject> GetPermissionQuery() => this.GetPermissionQuery(true);

    private IQueryable<TDomainObject> GetPermissionQuery(bool applyCurrentUser) =>
        queryableSource.GetQueryable<TDomainObject>().Where(bindingInfo.Filter).Pipe(
            applyCurrentUser,
            q => q.Where(bindingInfo.PrincipalNamePath.Select(name => name == currentUser.Name)));

    public IEnumerable<string> GetAccessors(Expression<Func<TDomainObject, bool>> permissionFilter) =>
        this.GetPermissionQuery(false).Where(permissionFilter).Select(bindingInfo.PrincipalNamePath);

    private Dictionary<Type, List<Guid>> ConvertPermission(TDomainObject permission, IEnumerable<Type> securityTypes) =>
        securityTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType => bindingInfo.GetRestrictionsExpr(securityContextType).Eval(permission).ToList());
}
