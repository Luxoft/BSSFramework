using Anch.Core;
using Anch.SecuritySystem;
using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.DomainServices;
using Anch.SecuritySystem.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL.Services;

public class RootSecurityService(
    IServiceProvider serviceProvider,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecuritySystem securitySystem,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : IRootSecurityService
{
    public ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule) =>
        serviceProvider.GetRequiredService<IDomainSecurityService<TDomainObject>>().GetSecurityProvider(securityRule);

    public bool HasAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule) =>
        defaultCancellationTokenSource.RunSync(async ct => await this.GetSecurityProvider<TDomainObject>(securityRule).HasAccessAsync(domainObject, ct));

    public void CheckAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule) =>
        defaultCancellationTokenSource.RunSync(async ct => await this.GetSecurityProvider<TDomainObject>(securityRule)
                                                                     .CheckAccessAsync(domainObject, accessDeniedExceptionService, ct));

    public bool HasAccess(DomainSecurityRule securityRule) => defaultCancellationTokenSource.RunSync(ct => securitySystem.HasAccessAsync(securityRule, ct));

    public void CheckAccess(DomainSecurityRule securityRule) => defaultCancellationTokenSource.RunSync(ct => securitySystem.CheckAccessAsync(securityRule, ct));
}
