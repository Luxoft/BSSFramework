namespace Framework.SecuritySystem.Expanders;

public record DomainObjectSecurityModeInfo(Type DomainType, SecurityRule.DomainSecurityRule? ViewRule, SecurityRule.DomainSecurityRule? EditRule);
