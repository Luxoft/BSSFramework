using Framework.Core;
using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
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
        return this.GetDomainSecurityServiceBase<TDomainObject>().GetSecurityProvider(securityMode);
    }

    protected IDomainSecurityService<TDomainObject> GetDomainSecurityServiceBase<TDomainObject>()
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.Context.ServiceProvider.GetService<IDomainSecurityService<TDomainObject>>()
               ?? this.Context.ServiceProvider.GetRequiredService<INotImplementedDomainSecurityService<TDomainObject>>();
    }
}

public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> :
    RootSecurityService<TBLLContext, TPersistentDomainObjectBase>,

    IRootSecurityService<TPersistentDomainObjectBase, TSecurityOperationCode>

    where TBLLContext : class, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>,
    IAccessDeniedExceptionServiceContainer, IServiceProviderContainer
    where TSecurityOperationCode : struct, Enum
    where TPersistentDomainObjectBase : class
{
    protected RootSecurityService(TBLLContext context)
        : base(context)
    {
    }


    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
        where TDomainObject : TPersistentDomainObjectBase
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityOperation);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(TSecurityOperationCode securityOperationCode)
        where TDomainObject : TPersistentDomainObjectBase
    {
        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityOperationCode);
    }


    protected IDomainSecurityService<TDomainObject, TSecurityOperationCode> GetDomainSecurityService<TDomainObject>()
    {
        return this.Context.ServiceProvider.GetService<IDomainSecurityService<TDomainObject, TSecurityOperationCode>>()
               ?? this.Context.ServiceProvider
                      .GetRequiredService<INotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>>();
    }
}
