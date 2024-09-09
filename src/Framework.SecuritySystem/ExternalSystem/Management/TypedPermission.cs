using Framework.Core;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public record TypedPermission(
    Guid Id,
    SecurityRole SecurityRole,
    Period Period,
    string Comment,
    IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions,
    bool IsVirtual);
