using Framework.Core;
using SecuritySystem;

namespace Automation.Utils;

public record TestPermission(SecurityRole SecurityRole, Period Period, IReadOnlyDictionary<Type, Array> Restrictions)
{
    public TestPermission(SecurityRole securityRole)
        : this(securityRole, Period.Eternity)
    {
    }

    public TestPermission(SecurityRole securityRole, Period period)
        : this(securityRole, period, new Dictionary<Type, Array>())
    {
    }

    public static implicit operator TestPermission(SecurityRole securityRole)
    {
        return new TestPermission(securityRole);
    }
}
