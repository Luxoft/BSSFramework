using Framework.Authorization.Domain;
using SecuritySystem.Credential;

namespace Framework.Authorization.SecuritySystem;

public interface IPrincipalResolver
{
    Task<Principal> Resolve(UserCredential userCredential, CancellationToken cancellationToken = default);
}
