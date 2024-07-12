namespace Framework.SecuritySystem;

public class SecurityAccessorResultOptimizer : SecurityAccessorResultVisitor, ISecurityAccessorResultOptimizer
{
    public SecurityAccessorResult Optimize(SecurityAccessorResult result)
    {
        return this.Visit(result);
    }

    public override SecurityAccessorResult Visit(SecurityAccessorResult.AndSecurityAccessorResult result)
    {
        if (result.Left is SecurityAccessorResult.InfinitySecurityAccessorResult)
        {
            return result.Right;
        }
        else if (result.Right is SecurityAccessorResult.InfinitySecurityAccessorResult)
        {
            return result.Left;
        }
        else if (result.Left is SecurityAccessorResult.FixedSecurityAccessorResult left
                 && result.Right is SecurityAccessorResult.FixedSecurityAccessorResult right)
        {
            return SecurityAccessorResult.Return(left.Items.Intersect(right.Items, StringComparer.CurrentCultureIgnoreCase));
        }
        else
        {
            return result;
        }
    }

    public override SecurityAccessorResult Visit(SecurityAccessorResult.OrSecurityAccessorResult result)
    {
        if (result.Left is SecurityAccessorResult.InfinitySecurityAccessorResult)
        {
            return result.Left;
        }
        else if (result.Right is SecurityAccessorResult.InfinitySecurityAccessorResult)
        {
            return result.Right;
        }
        else if (result.Left is SecurityAccessorResult.FixedSecurityAccessorResult left
                 && result.Right is SecurityAccessorResult.FixedSecurityAccessorResult right)
        {
            return SecurityAccessorResult.Return(left.Items.Union(right.Items, StringComparer.CurrentCultureIgnoreCase));
        }
        else
        {
            return result;
        }
    }

    public override SecurityAccessorResult Visit(SecurityAccessorResult.NegateSecurityAccessorResult result)
    {
        switch (result.BaseResult)
        {
            case SecurityAccessorResult.InfinitySecurityAccessorResult:
                return SecurityAccessorResult.Empty;

            case SecurityAccessorResult.FixedSecurityAccessorResult([]):
                return SecurityAccessorResult.Infinity;

            default:
                return result;
        }
    }
}
