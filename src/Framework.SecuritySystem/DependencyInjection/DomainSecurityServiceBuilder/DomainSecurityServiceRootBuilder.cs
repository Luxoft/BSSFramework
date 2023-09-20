using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

internal class DomainSecurityServiceRootBuilder<TIdent> : IDomainSecurityServiceRootBuilder<TIdent>
{
    private readonly List<IDomainSecurityServiceBuilder> domainBuilders = new();

    public IDomainSecurityServiceRootBuilder<TIdent> Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject, TIdent>> setup)
        where TDomainObject : IIdentityObject<TIdent>
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
