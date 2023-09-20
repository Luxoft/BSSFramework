using Framework.Persistent;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceRootBuilder<in TIdent>
{
    IDomainSecurityServiceRootBuilder<TIdent> Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject, TIdent>> setup)
        where TDomainObject : IIdentityObject<TIdent>;
}
