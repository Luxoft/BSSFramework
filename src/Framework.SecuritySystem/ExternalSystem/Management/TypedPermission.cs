﻿using Framework.Core;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public record TypedPermission(
    Guid Id,
    bool IsVirtual,
    SecurityRole SecurityRole,
    Period Period,
    string Comment,
    IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions);
