using Framework.Core;
using Framework.SecuritySystem;

namespace Automation.Utils;

public record TestPermission(SecurityRole SecurityRole, Period Period, IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions)
{
    public TestPermission(SecurityRole securityRole)
        : this(securityRole, Period.Eternity)
    {
    }

    public TestPermission(SecurityRole securityRole, Period period)
        : this(securityRole, period, new Dictionary<Type, IReadOnlyList<Guid>>())
    {
    }

    public static implicit operator TestPermission(SecurityRole securityRole)
    {
        return new TestPermission(securityRole);
    }
}
