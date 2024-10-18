namespace Framework.SecuritySystem.DiTests;

public record TestPermission(SecurityRole SecurityRole, IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions);
