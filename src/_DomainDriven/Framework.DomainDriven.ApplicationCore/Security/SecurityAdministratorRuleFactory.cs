using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.Security;

public class SecurityAdministratorRuleFactory(SecurityAdministratorRuleInfo securityAdministratorRuleInfo)
    : IFactory<DomainSecurityRule.RoleBaseSecurityRule>
{
    public DomainSecurityRule.RoleBaseSecurityRule Create() => securityAdministratorRuleInfo.SecurityRole;
}
