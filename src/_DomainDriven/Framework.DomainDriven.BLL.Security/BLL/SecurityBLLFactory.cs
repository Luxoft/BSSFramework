using CommonFramework;

using Framework.Core;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Providers;

namespace Framework.DomainDriven.BLL.Security;

public abstract class SecurityBLLFactory<TBLLContext, TBLL, TBLLImpl, TDomainObject>(TBLLContext context) :
    BLLContextContainer<TBLLContext>(context),
    ISecurityBLLFactory<TBLL, TDomainObject>
    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService<TDomainObject>>, IServiceProviderContainer
    where TDomainObject : class
    where TBLLImpl : TBLL
{
    public TBLL Create()
    {
        return this.Create(SecurityRule.Disabled);
    }

    public TBLL Create(SecurityRule securityRule)
    {
        return this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityRule));
    }

    public virtual TBLL Create(ISecurityProvider<TDomainObject> securityProvider)
    {
        return this.Context.ServiceProvider.GetRequiredService<IServiceProxyFactory>().Create<TBLL, TBLLImpl>(securityProvider);
    }
}
