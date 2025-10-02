using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

using SecuritySystem.Attributes;
using SecuritySystem.SecurityAccessor;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationAccessorInfinityStorage([DisabledSecurity] IRepository<Principal> principalRepository)
    : ISecurityAccessorInfinityStorage
{
    public IEnumerable<string> GetInfinityData() => principalRepository.GetQueryable().Select(p => p.Name);
}
