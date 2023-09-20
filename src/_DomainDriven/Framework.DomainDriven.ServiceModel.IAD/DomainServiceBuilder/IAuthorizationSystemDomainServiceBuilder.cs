using System.Linq.Expressions;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public interface IAuthorizationSystemDomainServiceBuilder<TDomainObject>
{
    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetView(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetEdit(SecurityOperation securityOperation);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetPath(SecurityPath<TDomainObject> securityPath);

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetDependency<TSource>(Expression<Func<TDomainObject, TSource>> securityPath);

    /// <summary>
    /// For projection
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetUntypedDependency<TSource>();

    IAuthorizationSystemDomainServiceBuilder<TDomainObject> SetCustomService<TDomainSecurityService>()
        where TDomainSecurityService : IDomainSecurityService<TDomainObject>;
}

public interface IAuthorizationSystemDomainServiceBuilder
{
    void Register(IServiceCollection services);
}
