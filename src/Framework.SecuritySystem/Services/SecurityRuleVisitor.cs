namespace Framework.SecuritySystem.Services;

using static DomainSecurityRule;

public abstract class SecurityRuleVisitor
{
    protected virtual DomainSecurityRule Visit(ExpandedRolesSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual DomainSecurityRule Visit(NonExpandedRolesSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual DomainSecurityRule Visit(DomainModeSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual DomainSecurityRule Visit(SecurityRuleHeader securityRule)
    {
        return securityRule;
    }

    protected virtual DomainSecurityRule Visit(OperationSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual DomainSecurityRule Visit(OrSecurityRule baseSecurityRule)
    {
        var visitedLeft = this.Visit(baseSecurityRule.Left);

        var visitedRight = this.Visit(baseSecurityRule.Right);

        if (baseSecurityRule.Left == visitedLeft && baseSecurityRule.Right == visitedRight)
        {
            return baseSecurityRule;
        }
        else
        {
            return visitedLeft.Or(visitedRight);
        }
    }

    protected virtual DomainSecurityRule Visit(AndSecurityRule baseSecurityRule)
    {
        var visitedLeft = this.Visit(baseSecurityRule.Left);

        var visitedRight = this.Visit(baseSecurityRule.Right);

        if (baseSecurityRule.Left == visitedLeft && baseSecurityRule.Right == visitedRight)
        {
            return baseSecurityRule;
        }
        else
        {
            return visitedLeft.And(visitedRight);
        }
    }

    protected virtual DomainSecurityRule Visit(NegateSecurityRule baseSecurityRule)
    {
        var visitedInner = this.Visit(baseSecurityRule.InnerRule);

        if (baseSecurityRule.InnerRule == visitedInner)
        {
            return baseSecurityRule;
        }
        else
        {
            return visitedInner.Negate();
        }
    }

    protected virtual DomainSecurityRule Visit(RoleBaseSecurityRule baseSecurityRule) => baseSecurityRule switch
    {
        ExpandedRolesSecurityRule securityRule => this.Visit(securityRule),
        NonExpandedRolesSecurityRule securityRule => this.Visit(securityRule),
        OperationSecurityRule securityRule => this.Visit(securityRule),
        _ => baseSecurityRule
    };

    public virtual DomainSecurityRule Visit(DomainSecurityRule baseSecurityRule) => baseSecurityRule switch
    {
        RoleBaseSecurityRule securityRule => this.Visit(securityRule),
        DomainModeSecurityRule securityRule => this.Visit(securityRule),
        SecurityRuleHeader securityRule => this.Visit(securityRule),
        OrSecurityRule securityRule => this.Visit(securityRule),
        AndSecurityRule securityRule => this.Visit(securityRule),
        NegateSecurityRule securityRule => this.Visit(securityRule),
        _ => baseSecurityRule
    };
}
