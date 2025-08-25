using Framework.Authorization.Domain;
using Framework.Core;
using SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Initialize;

public interface IAuthorizationSecurityContextInitializer : ISecurityInitializer
{
    new Task<MergeResult<SecurityContextType, SecurityContextInfo>> Init(CancellationToken cancellationToken = default);
}
