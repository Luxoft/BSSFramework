using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using SecuritySystem.Attributes;
using SecuritySystem.Credential;

namespace Framework.Authorization.SecuritySystemImpl;

public class PrincipalResolver([DisabledSecurity] IRepository<Principal> principalRepository) : IPrincipalResolver
{
    public async Task<Principal> Resolve(UserCredential userCredential, CancellationToken cancellationToken)
    {
        switch (userCredential)
        {
            case UserCredential.IdentUserCredential { Id: var id }:
                return await principalRepository.LoadAsync(id, cancellationToken);

            case UserCredential.NamedUserCredential { Name: var name }:
                return await principalRepository.GetQueryable().GenericSingleAsync(principal => principal.Name == name, cancellationToken);

            default:
                throw new ArgumentOutOfRangeException(nameof(userCredential));
        }
    }
}
