using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class PrincipalIdentitySource<TDomainObject>(
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<TDomainObject> domainRepository,
    PrincipalIdentitySourcePathInfo<TDomainObject> namePathInfo)
    : IPrincipalIdentitySource
    where TDomainObject : IIdentityObject<Guid>
{
    public Guid? TryGetId(string name)
    {
        return domainRepository.GetQueryable().Where(namePathInfo.Filter).SingleOrDefault(namePathInfo.Path.Select(v => v == name))?.Id;
    }
}
