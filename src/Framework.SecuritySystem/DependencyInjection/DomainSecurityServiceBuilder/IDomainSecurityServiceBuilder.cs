using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceBuilder<TDomainObject>
{
    IDomainSecurityServiceBuilder<TDomainObject> SetView(SecurityOperation securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetEdit(SecurityOperation securityRule);

    IDomainSecurityServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath);

    IDomainSecurityServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> securityPath);

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
