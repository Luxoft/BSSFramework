using System.Linq.Expressions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSource<TPrincipal, TPermission>(
    ICurrentUser currentUser,
    IQueryableSource queryableSource,
    TimeProvider timeProvider,
    VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo) : IPermissionSource<TPermission>
{
    private readonly Expression<Func<TPermission, string>> fullNamePath = bindingInfo.PrincipalPath.Select(bindingInfo.PrincipalNamePath);

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = this.GetPermissionQuery(true).ToList();

        return permissions.Select(permission => this.ConvertPermission(permission, securityTypes)).ToList();
    }

    public IQueryable<TPermission> GetPermissionQuery() => this.GetPermissionQuery(true);

    private IQueryable<TPermission> GetPermissionQuery(bool applyCurrentUser) =>
        queryableSource
            .GetQueryable<TPermission>()
            .Where(bindingInfo.Filter)
            .Pipe(
                bindingInfo.PeriodFilter != null,
                q =>
                {
                    var today = timeProvider.GetToday();

                    return q.Where(bindingInfo.PeriodFilter.Select(period => period.Contains(today)));
                })
            .Pipe(applyCurrentUser, q => q.Where(this.fullNamePath.Select(name => name == currentUser.Name)));

    public IEnumerable<string> GetAccessors(Expression<Func<TPermission, bool>> permissionFilter) =>
        this.GetPermissionQuery(false).Where(permissionFilter).Select(this.fullNamePath);

    private Dictionary<Type, List<Guid>> ConvertPermission(TPermission permission, IEnumerable<Type> securityTypes) =>
        securityTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType => bindingInfo.GetRestrictionsExpr(securityContextType).Eval(permission).ToList());
}
