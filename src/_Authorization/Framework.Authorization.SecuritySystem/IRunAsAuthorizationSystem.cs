using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IRunAsAuthorizationSystem : IAuthorizationSystem<Guid>
{
    IRunAsManager RunAsManager { get; }

    public bool IsAdmin(bool withRunAs);

    bool HasAccess(NonContextSecurityOperation securityOperation, bool withRunAs);

    void CheckAccess(NonContextSecurityOperation securityOperation, bool withRunAs);
}
