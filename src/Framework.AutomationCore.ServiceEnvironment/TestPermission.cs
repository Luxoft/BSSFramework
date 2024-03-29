﻿using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem.Bss;

namespace Automation.Utils;

public record TestPermission(string SecurityRoleName, Period Period, IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions)
{
    public TestPermission(SecurityRole securityRole)
        : this(securityRole.Name)
    {
    }

    public TestPermission(string securityRoleName)
        : this(securityRoleName, Period.Eternity)
    {
    }

    public TestPermission(string securityRoleName, Period period)
        : this(securityRoleName, period, new Dictionary<Type, IReadOnlyList<Guid>>())
    {
    }

    public static implicit operator TestPermission(SecurityRole securityRole)
    {
        return new TestPermission(securityRole);
    }

    public static TestPermission Administrator { get; } = new(BusinessRole.AdminRoleName);


    public static TestPermission SystemIntegration { get; } = new(BssSecurityOperation.SystemIntegration.Name);
}
