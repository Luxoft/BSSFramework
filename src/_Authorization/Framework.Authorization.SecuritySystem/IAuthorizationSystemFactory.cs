using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAuthorizationSystemFactory
{
    IAuthorizationSystem Create(bool withRunAs);
}
