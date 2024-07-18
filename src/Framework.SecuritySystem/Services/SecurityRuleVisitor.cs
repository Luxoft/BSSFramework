namespace Framework.SecuritySystem.Services;

public abstract class SecurityRuleVisitor
{
    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.OperationSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.DynamicRoleSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.ProviderSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.ProviderFactorySecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.ConditionSecurityRule securityRule)
    {
        return securityRule;
    }

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.OrSecurityRule baseSecurityRule)
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

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.AndSecurityRule baseSecurityRule)
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

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.NegateSecurityRule baseSecurityRule)
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

    protected virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.RoleBaseSecurityRule baseSecurityRule) => baseSecurityRule switch
    {
        SecurityRule.ExpandedRolesSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.NonExpandedRolesSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.OperationSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.DynamicRoleSecurityRule securityRule => this.Visit(securityRule),
        _ => throw new ArgumentOutOfRangeException(nameof(baseSecurityRule))
    };

    public virtual SecurityRule.DomainSecurityRule Visit(SecurityRule.DomainSecurityRule baseSecurityRule) => baseSecurityRule switch
    {
        SecurityRule.RoleBaseSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.ProviderSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.ProviderFactorySecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.ConditionSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.OrSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.AndSecurityRule securityRule => this.Visit(securityRule),
        SecurityRule.NegateSecurityRule securityRule => this.Visit(securityRule),
        _ => throw new ArgumentOutOfRangeException(nameof(baseSecurityRule))
    };
}
