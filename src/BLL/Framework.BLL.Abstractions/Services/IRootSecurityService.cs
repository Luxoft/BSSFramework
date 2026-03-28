using SecuritySystem;
using SecuritySystem.Providers;

namespace Framework.BLL.Services;

public interface IRootSecurityService
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule);

    bool HasAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule) =>
        this.GetSecurityProvider<TDomainObject>(securityRule).HasAccessAsync(domainObject).GetAwaiter().GetResult();

    void CheckAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule);
}
