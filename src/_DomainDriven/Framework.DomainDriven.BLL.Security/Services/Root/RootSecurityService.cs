using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<TBLLContext>, IRootSecurityService<TPersistentDomainObjectBase>

        where TBLLContext : class, IAccessDeniedExceptionServiceContainer
        where TPersistentDomainObjectBase : class
{
    private readonly IDictionaryCache<Type, object> domainSecurityServiceCache;


    protected RootSecurityService(TBLLContext context)
            : base(context)
    {
        this.domainSecurityServiceCache = new DictionaryCache<Type, object>(domainType =>

                                                                                    new Func<IDomainSecurityService<TPersistentDomainObjectBase>>(this.CreateDomainSecurityServiceBase<TPersistentDomainObjectBase>)
                                                                                            .CreateGenericMethod(domainType).Invoke(this, new object[0])).WithLock();
    }


    public virtual ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (securityMode == BLLSecurityMode.Disabled)
        {
            return this.GetDisabledSecurityProvider<TDomainObject>();
        }

        return this.GetDomainSecurityServiceBase<TDomainObject>().GetSecurityProvider(securityMode);
    }

    public virtual ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
    {
        return new DisabledSecurityProvider<TDomainObject>();
    }

    protected IDomainSecurityService<TDomainObject> GetDomainSecurityServiceBase<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (IDomainSecurityService<TDomainObject>)this.domainSecurityServiceCache[typeof(TDomainObject)];
    }

    protected virtual IDomainSecurityService<TDomainObject> CreateDomainSecurityServiceBase<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new NotImplementedDomainSecurityService<TDomainObject>(this);
    }
}


public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : RootSecurityService<TBLLContext, TPersistentDomainObjectBase>,

    IRootSecurityService<TPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContext : class, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>, IAccessDeniedExceptionServiceContainer, IServiceProviderContainer
        where TSecurityOperationCode : struct, Enum
        where TPersistentDomainObjectBase : class
{
    private readonly IDictionaryCache<Type, object> domainSecurityServiceCache;


    protected RootSecurityService(TBLLContext context)
            : base(context)
    {
        this.domainSecurityServiceCache = new DictionaryCache<Type, object>(domainType =>

                                                                                    new Func<IDomainSecurityService<TPersistentDomainObjectBase, TSecurityOperationCode>>(this.CreateDomainSecurityService<TPersistentDomainObjectBase>)
                                                                                            .CreateGenericMethod(domainType).Invoke(this, new object[0])).WithLock();
    }


    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityOperation);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(TSecurityOperationCode securityOperationCode)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityOperationCode);
    }


    public IDomainSecurityService<TDomainObject, TSecurityOperationCode> GetDomainSecurityService<TDomainObject>()
    {
        return (IDomainSecurityService<TDomainObject, TSecurityOperationCode>)this.domainSecurityServiceCache[typeof(TDomainObject)];
    }

    protected sealed override IDomainSecurityService<TDomainObject> CreateDomainSecurityServiceBase<TDomainObject>()
    {
        return this.GetDomainSecurityService<TDomainObject>();
    }

    protected virtual IDomainSecurityService<TDomainObject, TSecurityOperationCode> CreateDomainSecurityService<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return this.Context.ServiceProvider.GetService<IDomainSecurityService<TDomainObject, TSecurityOperationCode>>()
               ?? new NotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>(this);
    }
}
