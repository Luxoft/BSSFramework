namespace Framework.SecuritySystem.Expanders;

public record DomainObjectSecurityModeInfo(Type DomainType, DomainSecurityRule? ViewRule, DomainSecurityRule? EditRule);
