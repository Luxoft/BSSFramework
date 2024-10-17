using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Credential;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationUserCredentialNameByIdResolver([DisabledSecurity] IRepository<Principal> principalRepository)
    : IUserCredentialNameByIdResolver
{
    public string? TryGetUserName(Guid id)
    {
        return this.GetQueryable(id).Select(principal => (string?)principal.Name).SingleOrDefault();
    }

    private IQueryable<Principal> GetQueryable(Guid id) =>
        principalRepository.GetQueryable()
                           .Where(principal => principal.Id == id);
}
