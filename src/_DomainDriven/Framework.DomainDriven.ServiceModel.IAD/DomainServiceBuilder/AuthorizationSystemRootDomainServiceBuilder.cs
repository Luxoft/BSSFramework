using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD.DomainServiceBuilder;

public class AuthorizationSystemRootDomainServiceBuilder : IAuthorizationSystemRootDomainServiceBuilder
{
    private readonly List<IAuthorizationSystemDomainServiceBuilder> domainBuilders = new ();

    public IAuthorizationSystemRootDomainServiceBuilder Add<TDomainObject>(Action<IAuthorizationSystemDomainServiceBuilder<TDomainObject>> setup)
    {
        var builder = new AuthorizationSystemDomainServiceBuilder<TDomainObject>();

        setup(builder);

        this.domainBuilders.Add(builder);

        return this;
    }

    public void Register(IServiceCollection services)
    {
        this.domainBuilders.ForEach(b => b.Register(services));
    }
}
