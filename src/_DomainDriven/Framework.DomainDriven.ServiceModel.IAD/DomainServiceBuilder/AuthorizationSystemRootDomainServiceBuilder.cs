using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public class AuthorizationSystemRootDomainServiceBuilder<TIdent> : IAuthorizationSystemRootDomainServiceBuilder<TIdent>
{
    private readonly List<IAuthorizationSystemDomainServiceBuilder> domainBuilders = new ();

    public IAuthorizationSystemRootDomainServiceBuilder<TIdent> Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent>> setup)
        where TDomainObject : IIdentityObject<TIdent>
    {
        var builder = new AuthorizationSystemDomainServiceBuilder<TDomainObject, TIdent>();

        setup(builder);

        this.domainBuilders.Add(builder);

        return this;
    }

    public void Register(IServiceCollection services)
    {
        this.domainBuilders.ForEach(b => b.Register(services));
    }
}
