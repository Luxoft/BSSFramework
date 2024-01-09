using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem.Bss;

namespace Automation.Utils;

public class TestPermission
{
    public TestPermission(SecurityRole securityRole)
        : this(securityRole.Name)
    {
    }

    public TestPermission(string securityRoleName)
    {
        this.SecurityRoleName = securityRoleName;
    }

    public virtual string SecurityRoleName { get; }

    public virtual Period Period { get; } = Period.Eternity;

    public IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions => this.InternalRestrictions;

    protected Dictionary<Type, IReadOnlyList<Guid>> InternalRestrictions { get; } = new();

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
            this.InternalRestrictions[type] = new List<Guid>();
        }
        else
        {
            this.InternalRestrictions[type] = new List<Guid> { map(value.Value) };
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
            this.InternalRestrictions[type] = new List<Guid>();
        }
        else
        {
            this.InternalRestrictions[type] = new List<Guid> { map(value) };
        }
    }

    public static implicit operator TestPermission(SecurityRole securityRole)
    {
        return new TestPermission(securityRole);
    }


    public static TestPermission Administrator { get; } = new(BusinessRole.AdminRoleName);


    public static TestPermission SystemIntegration { get; } = new(BssSecurityOperation.SystemIntegration.Name);
}
