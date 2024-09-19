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
    SecurityRuleCredential defaultSecurityRuleCredential) : IPermissionSource<TPermission>
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
        return queryableSource
               .GetQueryable<TPermission>()
               .Where(bindingInfo.Filter)
               .PipeMaybe(bindingInfo.PeriodFilter, (q, filter) =>
                   {
                       var today = timeProvider.GetToday();

                       return q.Where(filter.Select(period => period.Contains(today)));
                   })
               .PipeMaybe(this.GetFiltrationPrincipalName(customSecurityRuleCredential ?? defaultSecurityRuleCredential),
                          (q, principalName) => q.Where(this.fullNamePath.Select(name => name == principalName)));
    }

    public IEnumerable<string> GetAccessors(Expression<Func<TPermission, bool>> permissionFilter) =>
        this.GetPermissionQuery(SecurityRuleCredential.AnyUser).Where(permissionFilter).Select(this.fullNamePath);

    private Dictionary<Type, List<Guid>> ConvertPermission(TPermission permission, IEnumerable<Type> securityTypes) =>
        securityTypes.ToDictionary(
            securityContextType => securityContextType,
            securityContextType => bindingInfo.GetRestrictionsExpr(securityContextType).Eval(permission).ToList());

    private string? GetFiltrationPrincipalName(SecurityRuleCredential securityRuleCredential)
    {
        switch (securityRuleCredential)
        {
            case SecurityRuleCredential.CustomPrincipalSecurityRuleCredential customPrincipalSecurityRuleCredential:
                return customPrincipalSecurityRuleCredential.Name;

            case { } when securityRuleCredential == SecurityRuleCredential.CurrentUserWithRunAs:
                return currentUser.Name;

            case { } when securityRuleCredential == SecurityRuleCredential.CurrentUserWithoutRunAs:
                return userAuthenticationService.GetUserName();

            case { } when securityRuleCredential == SecurityRuleCredential.AnyUser:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRuleCredential));
        }
    }
}
