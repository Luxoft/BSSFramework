using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityAccessorResolver
{
    IEnumerable<string> Resolve(SecurityAccessorData data);
}
