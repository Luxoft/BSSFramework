using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;

namespace Framework.BLL.Services;

public interface IRootSecurityService
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule);

    bool HasAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule);

    void CheckAccess<TDomainObject>(TDomainObject domainObject, SecurityRule securityRule);

    bool HasAccess(DomainSecurityRule securityRule);

    void CheckAccess(DomainSecurityRule securityRule);
}

