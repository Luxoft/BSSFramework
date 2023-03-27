using Framework.SecuritySystem;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class BLLFactoryBase<TBLLContext, TBLL, TBLLImpl, TDomainObject> : BLLContextContainer<TBLLContext>, ISecurityBLLFactory<TBLL, ISecurityProvider<TDomainObject>>

        where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TDomainObject>>, IServiceProviderContainer
        where TBLLImpl : TBLL
        where TDomainObject : class
{
    protected BLLFactoryBase([NotNull] TBLLContext context) : base(context)
    {
    }

    public TBLL Create()
    {
        var disabledProvider = this.Context.SecurityService.GetSecurityProvider<TDomainObject>(BLLSecurityMode.Disabled);

        return this.Create(disabledProvider);
    }

    public virtual TBLL Create(ISecurityProvider<TDomainObject> securityProvider)
    {
        return ActivatorUtilities.CreateInstance<TBLLImpl>(this.Context.ServiceProvider, securityProvider);
    }
}


public abstract class SecurityBLLFactoryBase<TBLLContext, TBLL, TBLLImpl, TDomainObject> : BLLFactoryBase<TBLLContext, TBLL, TBLLImpl, TDomainObject>, ISecurityBLLFactory<TBLL, BLLSecurityMode>

        where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TDomainObject>>, IServiceProviderContainer
        where TDomainObject : class
        where TBLLImpl : TBLL
{
    protected SecurityBLLFactoryBase([NotNull] TBLLContext context)
            : base(context)
    {
    }

    public TBLL Create(BLLSecurityMode securityMode)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityMode));
    }
}


public abstract class SecurityBLLFactory<TBLLContext, TBLL, TBLLImpl, TDomainObject, TSecurityOperationCode> : SecurityBLLFactoryBase<TBLLContext, TBLL, TBLLImpl, TDomainObject>, ISecurityBLLFactory<TBLL, TSecurityOperationCode>, ISecurityBLLFactory<TBLL, SecurityOperation<TSecurityOperationCode>>

        where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TDomainObject, TSecurityOperationCode>>, IServiceProviderContainer
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
        where TBLLImpl : TBLL
{
    protected SecurityBLLFactory([NotNull] TBLLContext context)
            : base(context)
    {
    }

    public TBLL Create(TSecurityOperationCode securityOperationCode)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityOperationCode));
    }

    public TBLL Create(SecurityOperation<TSecurityOperationCode> securityOperation)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityOperation));
    }
}
