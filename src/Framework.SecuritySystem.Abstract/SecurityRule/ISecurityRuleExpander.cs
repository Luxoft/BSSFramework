#nullable enable

namespace Framework.SecuritySystem;

public interface ISecurityRuleExpander
{
    SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule);
}
