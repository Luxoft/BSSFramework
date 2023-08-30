using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<TBLLContext>, IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>

        where TBLLContext : class, IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>
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

    public virtual ISecurityProvider<TDomainObject> GetNotImplementedSecurityProvider<TDomainObject>(object data)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        throw new Exception($"SecurityProvider not implemented for {typeof(TDomainObject).Name}");
    }

    public virtual ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new DisabledSecurityProvider<TDomainObject>(this.Context.AccessDeniedExceptionService);
    }


    protected IDomainSecurityService<TDomainObject> GetDomainSecurityServiceBase<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (IDomainSecurityService<TDomainObject>)this.domainSecurityServiceCache[typeof(TDomainObject)];
    }

    protected virtual IDomainSecurityService<TDomainObject> CreateDomainSecurityServiceBase<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return new NotImplementedDomainSecurityService<TBLLContext, TDomainObject>(this);
    }

    IDomainSecurityService<TDomainObject> IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>.GetDomainSecurityService<TDomainObject>()
    {
        return this.GetDomainSecurityServiceBase<TDomainObject>();
    }
}


public abstract class RootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : RootSecurityService<TBLLContext, TPersistentDomainObjectBase>,

    IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContext : class, ISecurityOperationResolver<TPersistentDomainObjectBase, TSecurityOperationCode>, IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>, IServiceProviderContainer
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
               ?? new NotImplementedDomainSecurityService<TBLLContext, TDomainObject, TSecurityOperationCode>(this);
    }
}
