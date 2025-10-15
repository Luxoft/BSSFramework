using Framework.Authorization.Domain;

using SecuritySystem.Credential;

namespace Framework.Authorization.SecuritySystemImpl;

public interface IPrincipalResolver
{
    Task<Principal> Resolve(UserCredential userCredential, CancellationToken cancellationToken = default);
}
