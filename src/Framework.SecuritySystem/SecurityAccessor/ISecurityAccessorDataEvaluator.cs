namespace Framework.SecuritySystem.SecurityAccessor;

public interface ISecurityAccessorDataEvaluator
{
    IEnumerable<string> Evaluate(SecurityAccessorData securityAccessorData);
}
