namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public interface IAuthorizationSystemRootDomainServiceBuilder
{
    IAuthorizationSystemRootDomainServiceBuilder Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject>> setup);
}
