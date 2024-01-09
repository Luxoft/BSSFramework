using Framework.Authorization.SecuritySystem;
using Framework.Core;

namespace Automation.Utils;

public class TestPermissionBuilder
{
    public TestPermissionBuilder(SecurityRole securityRole)
        : this(securityRole.Name)
    {
    }

    public TestPermissionBuilder(string securityRoleName)
    {
        this.SecurityRoleName = securityRoleName;
    }

    public TestPermissionBuilder()
    {
    }

    public string SecurityRoleName { get; set; }

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

    protected void SetSingleIdentityC<TIdentity>(Type type, Func<TIdentity, Guid> map, TIdentity value)
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


    public static implicit operator TestPermission(TestPermissionBuilder securityRole)
    {
        if (securityRole.SecurityRoleName == null)
        {
            throw new InvalidOperationException($"{nameof(securityRole.SecurityRoleName)} not initialized");
        }

        return new TestPermission(
            securityRole.SecurityRoleName,
            securityRole.Period,
            securityRole.Restrictions.ChangeValue(v => v.ToReadOnlyListI()));
    }
}
