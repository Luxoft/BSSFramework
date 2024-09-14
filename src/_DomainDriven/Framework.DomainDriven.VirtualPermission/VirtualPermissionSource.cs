using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.Services;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSource<TPrincipal, TPermission>(
    ICurrentUser currentUser,
    IUserAuthenticationService userAuthenticationService,
    IQueryableSource queryableSource,
    TimeProvider timeProvider,
    VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo,
    SecurityRuleCredential securityRuleCredential) : IPermissionSource<TPermission>
{
    private readonly Expression<Func<TPermission, string>> fullNamePath = bindingInfo.PrincipalPath.Select(bindingInfo.PrincipalNamePath);

    public bool HasAccess() => this.GetPermissionQuery().Any();

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes)
    {
        var permissions = this.GetPermissionQuery(null).ToList();

        return permissions.Select(permission => this.ConvertPermission(permission, securityTypes)).ToList();
    }

    public IQueryable<TPermission> GetPermissionQuery() => this.GetPermissionQuery(null);

    private IQueryable<TPermission> GetPermissionQuery(SecurityRuleCredential? customSecurityRuleCredential)
    {
        var principalName = this.GetFiltrationPrincipalName(customSecurityRuleCredential);

        return queryableSource
               .GetQueryable<TPermission>()
               .Where(bindingInfo.Filter)
               .Pipe(
                   bindingInfo.PeriodFilter != null,
                   q =>
                   {
                       var today = timeProvider.GetToday();

                       return q.Where(bindingInfo.PeriodFilter.Select(period => period.Contains(today)));
                   })
               .Pipe(principalName != null, q => q.Where(this.fullNamePath.Select(name => name == principalName)));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<TPermission, bool>> permissionFilter) =>
        this.GetPermissionQuery(SecurityRuleCredential.AnyUser).Where(permissionFilter).Select(this.fullNamePath);

    private Dictionary<Type, List<Guid>> ConvertPermission(TPermission permission, IEnumerable<Type> securityTypes) =>
        securityTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType => bindingInfo.GetRestrictionsExpr(securityContextType).Eval(permission).ToList());

    private string? GetFiltrationPrincipalName(SecurityRuleCredential? customSecurityRuleCredential)
    {
        var credential = customSecurityRuleCredential ?? securityRuleCredential;

        switch (credential)
        {
            case SecurityRuleCredential.CustomPrincipalSecurityRuleCredential customPrincipalSecurityRuleCredential:
                return customPrincipalSecurityRuleCredential.Name;

            case { } when credential == SecurityRuleCredential.CurrentUserWithRunAs:
                return currentUser.Name;

            case { } when credential == SecurityRuleCredential.CurrentUserWithoutRunAs:
                return userAuthenticationService.GetUserName();

            case { } when credential == SecurityRuleCredential.AnyUser:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(credential));
        }
    }
}
