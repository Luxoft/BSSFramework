using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLFactory<TBLLContext, TBLL, TBLLImpl, TDomainObject> :
    BLLContextContainer<TBLLContext>,
    ISecurityBLLFactory<TBLL, TDomainObject>

    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TDomainObject>>, IServiceProviderContainer
    where TDomainObject : class
    where TBLLImpl : TBLL
{
    protected SecurityBLLFactory(TBLLContext context)
        : base(context)
    {
    }

    public TBLL Create()
    {
        var disabledProvider = this.Context.SecurityService.GetSecurityProvider<TDomainObject>(SecurityRule.Disabled);

        return this.Create(disabledProvider);
    }

    public TBLL Create(SecurityRule securityRule)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityRule));
    }

    public TBLL Create(SecurityRule securityRule)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityRule));
    }

    public virtual TBLL Create(ISecurityProvider<TDomainObject> securityProvider)
    {
        return ActivatorUtilities.CreateInstance<TBLLImpl>(this.Context.ServiceProvider, securityProvider);
    }
}
