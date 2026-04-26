using Anch.Core;

using Framework.BLL.Services;
using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;

namespace Framework.BLL;

public abstract class SecurityBLLFactory<TBLLContext, TBLL, TBLLImpl, TDomainObject>(TBLLContext context) :
    BLLContextContainer<TBLLContext>(context),
    ISecurityBLLFactory<TBLL, TDomainObject>
    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService>, IServiceProviderContainer
    where TDomainObject : class
    where TBLLImpl : TBLL
{
    public TBLL Create() => this.Create(SecurityRule.Disabled);

    public TBLL Create(SecurityRule securityRule) => this.Create(this.Context.SecurityService.GetSecurityProvider<TDomainObject>(securityRule));

    public virtual TBLL Create(ISecurityProvider<TDomainObject> securityProvider) => this.Context.ServiceProvider.GetRequiredService<IServiceProxyFactory>().Create<TBLL, TBLLImpl>(securityProvider);
}
