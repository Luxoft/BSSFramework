using System.Linq.Expressions;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceBuilder<TDomainObject, in TIdent>
    where TDomainObject : IIdentityObject<TIdent>
{
    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetView(SecurityOperation securityOperation);

    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetEdit(SecurityOperation securityOperation);

    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetPath(SecurityPath<TDomainObject> securityPath);

    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> securityPath);

    /// <summary>
    /// For projection
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetUntypedDependency<TSource>()
        where TSource : class, IIdentityObject<TIdent>;

    IDomainSecurityServiceBuilder<TDomainObject, TIdent> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>;
}

public interface IDomainSecurityServiceBuilder
{
    void Register(IServiceCollection services);
}
