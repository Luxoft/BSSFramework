using System.Linq.Expressions;

using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public interface IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent>
    where TDomainObject : IIdentityObject<TIdent>
{
    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetView(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetEdit(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetPath(SecurityPath<TDomainObject> securityPath);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> securityPath);

    /// <summary>
    /// For projection
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetUntypedDependency<TSource>()
        where TSource : class, IIdentityObject<TIdent>;

    IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>;
}

public interface IAuthorizationSystemDomainServiceBuilder
{
    void Register(IServiceCollection services);
}
