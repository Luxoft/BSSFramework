using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAuthorizationSystemFactory
{
    ISecuritySystem Create(bool withRunAs);
}
