using CommonFramework;

using Framework.BLL.Services;
using Framework.BLL.Services.Root._Base;
using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.Providers;

namespace Framework.BLL.BLL;

public abstract class SecurityBLLFactory<TBLLContext, TBLL, TBLLImpl, TDomainObject>(TBLLContext context) :
    BLLContextContainer<TBLLContext>(context),
    ISecurityBLLFactory<TBLL, TDomainObject>
    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService>, IServiceProviderContainer
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
