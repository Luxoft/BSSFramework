#nullable enable

namespace Framework.SecuritySystem;

public record DomainObjectSecurityModeInfo(Type DomainType, IEnumerable<SecurityRule> ViewRules, IEnumerable<SecurityRule> EditRules);
