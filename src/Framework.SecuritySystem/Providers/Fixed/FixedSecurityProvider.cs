using System.Linq.Expressions;

namespace Framework.SecuritySystem;

/// <summary>
/// Провайдер доступа с фиксированным ответом для одного типа
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract class FixedSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>
        where TDomainObject : class
{
    private readonly Lazy<bool> hasAccessLazy;

    private readonly Lazy<Expression<Func<TDomainObject, bool>>> securityFilterLazy;


    protected FixedSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            : base(accessDeniedExceptionService)
    {
        this.hasAccessLazy = new Lazy<bool>(this.HasAccess);

        this.securityFilterLazy = new Lazy<Expression<Func<TDomainObject, bool>>>(() =>
                                                                                  {
                                                                                      var hasAccess = this.hasAccessLazy.Value;

                                                                                      // For hibernate
                                                                                      if (hasAccess) { return _ => true; }
                                                                                      else { return _ => false; }
                                                                                  });
    }


    protected abstract bool HasAccess();


    public override bool HasAccess(TDomainObject _)
    {
        return this.hasAccessLazy.Value;
    }

    public override Expression<Func<TDomainObject, bool>> SecurityFilter
    {
        get { return this.securityFilterLazy.Value; }
    }
}
