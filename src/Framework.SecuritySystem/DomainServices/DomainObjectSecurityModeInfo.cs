#nullable enable

namespace Framework.SecuritySystem;

public record DomainObjectSecurityModeInfo(
    Type DomainType,
    SecurityRule.DomainObjectSecurityRule? ViewRule,
    SecurityRule.DomainObjectSecurityRule? EditRule,
    SecurityRule.DomainObjectSecurityRule? RemoveRule);
