namespace Framework.SecuritySystem;

public abstract class SecurityAccessorResultVisitor
{
    public virtual SecurityAccessorResult Visit(SecurityAccessorResult baseResult)
    {
        switch (baseResult)
        {
            case SecurityAccessorResult.FixedSecurityAccessorResult result:
                return this.Visit(result);

            case SecurityAccessorResult.AndSecurityAccessorResult result:
                return this.Visit(result);

            case SecurityAccessorResult.OrSecurityAccessorResult result:
                return this.Visit(result);

            case SecurityAccessorResult.InfinitySecurityAccessorResult result:
                return this.Visit(result);

            case SecurityAccessorResult.NegateSecurityAccessorResult result:
                return this.Visit(result);

            default:
                throw new ArgumentOutOfRangeException(nameof(baseResult));
        }
    }

    public virtual SecurityAccessorResult Visit(SecurityAccessorResult.FixedSecurityAccessorResult result)
    {
        return result;
    }

    public virtual SecurityAccessorResult Visit(SecurityAccessorResult.AndSecurityAccessorResult result)
    {
        return result;
    }

    public virtual SecurityAccessorResult Visit(SecurityAccessorResult.OrSecurityAccessorResult result)
    {
        return result;
    }

    public virtual SecurityAccessorResult Visit(SecurityAccessorResult.InfinitySecurityAccessorResult result)
    {
        return result;
    }

    public virtual SecurityAccessorResult Visit(SecurityAccessorResult.NegateSecurityAccessorResult result)
    {
        return result;
    }
}
