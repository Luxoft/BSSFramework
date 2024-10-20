using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceBuilder<TDomainObject>
{
    IDomainSecurityServiceBuilder<TDomainObject> SetView(DomainSecurityRule securityRule) => this.SetMode(SecurityRule.View, securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetEdit(DomainSecurityRule securityRule) => this.SetMode(SecurityRule.Edit, securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetMode(SecurityRule.ModeSecurityRule modeSecurityRule, DomainSecurityRule implementedSecurityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath);

    /// <summary>
    /// RelativeDomainPathInfo must be registered
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>();

    /// <summary>
    /// RelativeDomainPathInfo will be automatically registered
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="relativeDomainPath"></param>
    /// <returns></returns>
    IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> relativeDomainPath);

    /// <summary>
    /// For projection
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IDomainSecurityServiceBuilder<TDomainObject> SetUntypedDependency<TSource>();

    IDomainSecurityServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>;

    IDomainSecurityServiceBuilder<TDomainObject> Override<TSecurityFunctor>()
        where TSecurityFunctor : IOverrideSecurityProviderFunctor<TDomainObject>;
}

public interface IDomainSecurityServiceBuilder
{
    void Register(IServiceCollection services);
}
