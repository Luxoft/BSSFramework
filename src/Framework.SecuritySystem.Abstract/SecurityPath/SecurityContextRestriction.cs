namespace Framework.SecuritySystem;

public record SecurityContextRestriction(Type Type, bool Required, string? Key, SecurityContextRestrictionFilterInfo? Filter);
