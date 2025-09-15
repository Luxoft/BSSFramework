using Framework.Core;
using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.DomainServices;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.BLL.Security;

public class RootSecurityService<TPersistentDomainObjectBase> : IRootSecurityService<TPersistentDomainObjectBase>, IServiceProviderContainer

    where TPersistentDomainObjectBase : class
{
    public RootSecurityService(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }

    public virtual ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule)
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityRule);
    }

    protected IDomainSecurityService<TDomainObject> GetDomainSecurityService<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.ServiceProvider.GetService<IDomainSecurityService<TDomainObject>>();
    }
}
