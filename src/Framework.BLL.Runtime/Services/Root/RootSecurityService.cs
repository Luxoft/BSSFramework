using Framework.BLL.Services.Root._Base;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.AccessDenied;
using SecuritySystem.DomainServices;
using SecuritySystem.Providers;

namespace Framework.BLL.Services.Root;

public class RootSecurityService(
    IServiceProvider serviceProvider,
    IAccessDeniedExceptionService accessDeniedExceptionService) : IRootSecurityService
{
    public virtual ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule) =>
        this.GetDomainSecurityService<TDomainObject>().GetSecurityProvider(securityRule);

    public void CheckAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule) =>
        this.GetSecurityProvider<TDomainObject>(securityRule).CheckAccessAsync(domainObject, accessDeniedExceptionService).GetAwaiter().GetResult();

    protected IDomainSecurityService<TDomainObject> GetDomainSecurityService<TDomainObject>() =>
        serviceProvider.GetRequiredService<IDomainSecurityService<TDomainObject>>();
}
