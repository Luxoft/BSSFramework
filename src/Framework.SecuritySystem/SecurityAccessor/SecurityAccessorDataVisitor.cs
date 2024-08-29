namespace Framework.SecuritySystem.SecurityAccessor;

public abstract class SecurityAccessorDataVisitor
{
    public virtual SecurityAccessorData Visit(SecurityAccessorData baseData)
    {
        switch (baseData)
        {
            case SecurityAccessorData.FixedSecurityAccessorData result:
                return this.Visit(result);

            case SecurityAccessorData.AndSecurityAccessorData result:
                return this.Visit(result);

            case SecurityAccessorData.OrSecurityAccessorData result:
                return this.Visit(result);

            case SecurityAccessorData.InfinitySecurityAccessorData result:
                return this.Visit(result);

            case SecurityAccessorData.NegateSecurityAccessorData result:
                return this.Visit(result);

            default:
                throw new ArgumentOutOfRangeException(nameof(baseData));
        }
    }

    public virtual SecurityAccessorData Visit(SecurityAccessorData.FixedSecurityAccessorData result)
    {
        return result;
    }

    public virtual SecurityAccessorData Visit(SecurityAccessorData.AndSecurityAccessorData result)
    {
        return result;
    }

    public virtual SecurityAccessorData Visit(SecurityAccessorData.OrSecurityAccessorData result)
    {
        return result;
    }

    public virtual SecurityAccessorData Visit(SecurityAccessorData.InfinitySecurityAccessorData result)
    {
        return result;
    }

    public virtual SecurityAccessorData Visit(SecurityAccessorData.NegateSecurityAccessorData result)
    {
        return result;
    }
}
