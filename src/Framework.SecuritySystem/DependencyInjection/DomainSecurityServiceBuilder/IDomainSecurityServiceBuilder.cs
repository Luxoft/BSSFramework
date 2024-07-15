using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceBuilder<TDomainObject>
{
    IDomainSecurityServiceBuilder<TDomainObject> SetView(SecurityRule.DomainSecurityRule securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetEdit(SecurityRule.DomainSecurityRule securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath);

    /// <summary>
    /// RelativeDomainPathInfo must be registered
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>();

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
