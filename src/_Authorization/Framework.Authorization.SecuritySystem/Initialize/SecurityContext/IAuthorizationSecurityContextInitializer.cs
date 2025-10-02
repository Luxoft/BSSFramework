using CommonFramework;

using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.SecuritySystemImpl.Initialize;

public interface IAuthorizationSecurityContextInitializer : ISecurityInitializer
{
    new Task<MergeResult<SecurityContextType, SecurityContextInfo>> Init(CancellationToken cancellationToken = default);
}
