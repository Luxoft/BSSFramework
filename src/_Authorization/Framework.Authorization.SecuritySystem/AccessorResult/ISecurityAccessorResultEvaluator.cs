using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityAccessorResultEvaluator
{
    IEnumerable<string> GetAccessors(SecurityAccessorResult securityAccessorResult);
}
