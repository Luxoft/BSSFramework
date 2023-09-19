using Framework.Core;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<TBLLContext>,
                                                                                      IRootSecurityService<TPersistentDomainObjectBase>

    where TBLLContext : class, IServiceProviderContainer
    where TPersistentDomainObjectBase : class
{
    protected RootSecurityService(TBLLContext context)
        : base(context)
    {
    }


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
        return this.Context.ServiceProvider.GetService<IDomainSecurityService<TDomainObject>>()
               ?? this.Context.ServiceProvider.GetRequiredService<INotImplementedDomainSecurityService<TDomainObject>>();
    }
}
