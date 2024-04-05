#nullable enable

namespace Framework.SecuritySystem;

public record DomainObjectSecurityModeInfo(Type DomainType, SecurityRule? ViewRule, SecurityRule? EditRule);
