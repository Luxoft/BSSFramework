namespace Framework.SecuritySystem;

public record DomainObjectSecurityModeInfo(Type DomainType, SecurityRule.DomainSecurityRule? ViewRule, SecurityRule.DomainSecurityRule? EditRule);
