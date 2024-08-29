namespace Framework.SecuritySystem.SecurityAccessor;

public class SecurityAccessorResolver(
    ISecurityAccessorDataOptimizer optimizer,
    ISecurityAccessorDataEvaluator evaluator) : ISecurityAccessorResolver
{
    public IEnumerable<string> Resolve(SecurityAccessorData data)
    {
        return evaluator.Evaluate(optimizer.Optimize(data));
    }
}
