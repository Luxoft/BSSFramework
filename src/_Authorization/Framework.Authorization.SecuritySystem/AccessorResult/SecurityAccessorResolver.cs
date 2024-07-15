using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityAccessorResolver(
    ISecurityAccessorDataOptimizer optimizer,
    ISecurityAccessorDataEvaluator evaluator) : ISecurityAccessorResolver
{
    public IEnumerable<string> Resolve(SecurityAccessorData data)
    {
        return evaluator.Evaluate(optimizer.Optimize(data));
    }
}
