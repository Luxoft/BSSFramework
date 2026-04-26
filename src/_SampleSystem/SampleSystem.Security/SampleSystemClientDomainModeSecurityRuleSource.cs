using SampleSystem.Domain;

using Anch.SecuritySystem.SecurityRuleInfo;

namespace SampleSystem.Security;

public class SampleSystemClientDomainModeSecurityRuleSource(IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList)
    : ClientDomainModeSecurityRuleSource(domainModeSecurityRuleInfoList)
{
    protected override bool Allowed(DomainModeSecurityRuleInfo info) => base.Allowed(info) && typeof(PersistentDomainObjectBase).IsAssignableFrom(info.SecurityRule.DomainType);
}
