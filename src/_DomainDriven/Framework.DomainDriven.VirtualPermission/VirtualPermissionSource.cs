using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem;
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

    public IQueryable<TDomainObject> GetPermissionQuery()
    {
        return this.GetPermissionQuery(false);
    }

    private IQueryable<TDomainObject> GetPermissionQuery(bool applyCurrentUser)
    {
        return queryableSource.GetQueryable<TDomainObject>().Where(bindingInfo.Filter).Pipe(
            applyCurrentUser,
            q => q.Where(bindingInfo.PrincipalNamePath.Select(name => name == currentUser.Name)));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<TDomainObject, bool>> permissionFilter) =>
        this.GetPermissionQuery().Where(permissionFilter).Select(bindingInfo.PrincipalNamePath);

    private Dictionary<Type, List<Guid>> ConvertPermission(TDomainObject permission, IEnumerable<Type> securityTypes)
    {
        return securityTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType => bindingInfo.GetPermissionRestrictions(securityContextType).Eval(permission).ToList());
    }
}
