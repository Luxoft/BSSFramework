using System.Linq.Expressions;

namespace Framework.SecuritySystem;

/// <summary>
/// Провайдер доступа с фиксированным ответом для одного типа
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract class FixedSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>
{
    private readonly Lazy<bool> hasAccessLazy;

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> securityFilterLazy;

    protected FixedSecurityProvider()
    {
        this.hasAccessLazy = new Lazy<bool>(this.HasAccess);

        this.securityFilterLazy = new Lazy<Expression<Func<TDomainObject, bool>>>(() => _ => this.hasAccessLazy.Value);
    }

    protected abstract bool HasAccess();

    public override bool HasAccess(TDomainObject _) => this.hasAccessLazy.Value;

    public override Expression<Func<TDomainObject, bool>> SecurityFilter => this.securityFilterLazy.Value;
}
