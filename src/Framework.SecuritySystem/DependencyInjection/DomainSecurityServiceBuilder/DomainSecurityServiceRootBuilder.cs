using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

internal class DomainSecurityServiceRootBuilder<TIdent> : IDomainSecurityServiceRootBuilder
{
    private readonly List<IDomainSecurityServiceBuilder> domainBuilders = new();

    public IDomainSecurityServiceRootBuilder Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject>> setup)
    {
        var builder = new DomainSecurityServiceBuilder<TDomainObject, TIdent>();

        setup(builder);

        this.domainBuilders.Add(builder);

        return this;
    }

    public void Register(IServiceCollection services)
    {
        this.domainBuilders.ForEach(b => b.Register(services));
    }
}
