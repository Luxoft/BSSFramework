using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityAccessorDataEvaluator
{
    IEnumerable<string> Evaluate(SecurityAccessorData securityAccessorData);
}
