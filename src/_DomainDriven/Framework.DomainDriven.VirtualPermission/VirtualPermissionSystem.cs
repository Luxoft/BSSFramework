using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.UserSource;

using NHibernate.Linq;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityContextSource securityContextSource,
    ICurrentUser currentUser,
    IQueryableSource queryableSource,
    VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermissionSystem
{
    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (securityRuleExpander.FullExpand(securityRule).SecurityRoles.Contains(bindingInfo.SecurityRole))
        {
            return new VirtualPermissionSource<TDomainObject>(securityContextSource, currentUser, queryableSource, bindingInfo);
        }
        else
        {
            return new EmptyPermissionSource();
        }
    }

    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) =>
        await this.GetPermissionSource(bindingInfo.SecurityRole).GetPermissionQuery().AnyAsync(cancellationToken)
            ? [bindingInfo.SecurityRole]
            : [];
}
