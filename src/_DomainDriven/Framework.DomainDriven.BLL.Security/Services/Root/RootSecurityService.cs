using Framework.Core;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public class RootSecurityService<TPersistentDomainObjectBase> : IRootSecurityService<TPersistentDomainObjectBase>

    where TPersistentDomainObjectBase : class
{
    private readonly IServiceProvider serviceProvider;

    public RootSecurityService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
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
        return this.serviceProvider.GetService<IDomainSecurityService<TDomainObject>>()
               ?? this.serviceProvider.GetRequiredService<INotImplementedDomainSecurityService<TDomainObject>>();
    }
}

public class RootSecurityService<TBLLContext, TPersistentDomainObjectBase> : RootSecurityService<TPersistentDomainObjectBase>, IBLLContextContainer<TBLLContext>

    where TBLLContext : class, IServiceProviderContainer
    where TPersistentDomainObjectBase : class
{
    public RootSecurityService(TBLLContext context)
        : base(context.ServiceProvider)
    {
        this.Context = context;
    }

    public TBLLContext Context { get; }
}
