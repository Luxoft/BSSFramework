using Framework.Core;
using Framework.SecuritySystem;

namespace Automation.Utils;

public record TestPermission(string SecurityRoleName, Period Period, IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions)
{
    public TestPermission(SecurityRole securityRole)
        : this(securityRole.Name)
    {
    }
    public TestPermission(SecurityRole securityRole, IReadOnlyDictionary<Type, IReadOnlyList<Guid>> restrictions)
        : this(securityRole.Name, Period.Eternity, restrictions)
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
}
