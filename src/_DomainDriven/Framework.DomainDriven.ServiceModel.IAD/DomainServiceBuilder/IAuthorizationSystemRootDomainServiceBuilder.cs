using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public interface IAuthorizationSystemRootDomainServiceBuilder<TIdent>
{
    IAuthorizationSystemRootDomainServiceBuilder<TIdent> Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent>> setup)
        where TDomainObject : IIdentityObject<TIdent>;
}
