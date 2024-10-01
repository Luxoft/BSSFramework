using Framework.Core;
using Framework.SecuritySystem;

namespace Automation.Utils;

public class TestPermissionBuilder
{
    public TestPermissionBuilder(SecurityRole securityRole)
        :this()
    {
        this.SecurityRole = securityRole;
    }

    public TestPermissionBuilder()
    {
    }

    public SecurityRole? SecurityRole { get; set; }

    public Period Period { get; set; } = Period.Eternity;

    public Dictionary<Type, List<Guid>> Restrictions { get; } = new();

    protected TIdentity? GetSingleIdentity<TIdentity>(Type type, Func<Guid, TIdentity> map)
        where TIdentity : struct
    {
        return this.Restrictions.GetValueOrDefault(type).MaybeToNullable(v => map(v.Single()));
    }

    protected void SetSingleIdentity<TIdentity>(Type type, Func<TIdentity, Guid> map, TIdentity? value)
        where TIdentity : struct
    {
        if (value == null)
        {
            this.Restrictions[type] = new List<Guid>();
        }
        else
        {
            this.Restrictions[type] = new List<Guid> { map(value.Value) };
        }
    }

    protected TIdentity GetSingleIdentityC<TIdentity>(Type type, Func<Guid, TIdentity> map)
        where TIdentity : class
    {
        return this.Restrictions.GetValueOrDefault(type).Maybe(v => map(v.Single()));
    }

    protected void SetSingleIdentityC<TIdentity>(Type type, Func<TIdentity, Guid> map, TIdentity? value)
        where TIdentity : class
    {
        if (value == null)
        {
            this.Restrictions[type] = new List<Guid>();
        }
        else
        {
            this.Restrictions[type] = new List<Guid> { map(value) };
        }
    }


    public static implicit operator TestPermission(TestPermissionBuilder testPermissionBuilder)
    {
        if (testPermissionBuilder.SecurityRole == null)
        {
            throw new InvalidOperationException($"{nameof(testPermissionBuilder.SecurityRole)} not initialized");
        }

        return new TestPermission(
            testPermissionBuilder.SecurityRole,
            testPermissionBuilder.Period,
            testPermissionBuilder.Restrictions.Where(pair => pair.Value.Any()).ToDictionary(pair => pair.Key, pair => pair.Value.ToReadOnlyListI()));
    }
}
