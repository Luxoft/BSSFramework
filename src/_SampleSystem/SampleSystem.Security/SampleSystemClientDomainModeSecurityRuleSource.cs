using Framework.SecuritySystem.SecurityRuleInfo;

using SampleSystem.Domain;

namespace SampleSystem.Security;

public class SampleSystemClientDomainModeSecurityRuleSource(IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList)
    : ClientDomainModeSecurityRuleSource(domainModeSecurityRuleInfoList)
{
    protected override bool Allowed(DomainModeSecurityRuleInfo info)
    {
        return base.Allowed(info) && typeof(PersistentDomainObjectBase).IsAssignableFrom(info.SecurityRule.DomainType);
    }
}
