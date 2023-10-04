using Framework.Core;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public class RootSecurityService<TPersistentDomainObjectBase> : IRootSecurityService<TPersistentDomainObjectBase>, IServiceProviderContainer

    where TPersistentDomainObjectBase : class
{
    public RootSecurityService(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }

    public virtual ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityMode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation securityOperation)
        where TDomainObject : TPersistentDomainObjectBase
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityOperation);
    }

    protected IDomainSecurityService<TDomainObject> GetDomainSecurityService<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.ServiceProvider.GetService<IDomainSecurityService<TDomainObject>>()
               ?? this.ServiceProvider.GetRequiredService<INotImplementedDomainSecurityService<TDomainObject>>();
    }
}
