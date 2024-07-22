using static Framework.SecuritySystem.SecurityRule;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleOptimizer : SecurityRuleVisitor, ISecurityRuleOptimizer
{
    protected override DomainSecurityRule Visit(OrSecurityRule baseSecurityRule)
    {
        var visitedBase = base.Visit(baseSecurityRule);

        return visitedBase switch
        {
            OrSecurityRule { Left: { } left, Right: { } right }
                when left == Disabled || right == Disabled => Disabled,

            OrSecurityRule { Left: ExpandedRolesSecurityRule left, Right: ExpandedRolesSecurityRule right }
                when left.CustomExpandType == right.CustomExpandType => left + right,

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: ExpandedRolesSecurityRule leftLeft, Right: { } leftRight },
                    Right: ExpandedRolesSecurityRule right
                }
                when leftLeft.CustomExpandType == right.CustomExpandType => leftRight.Or(leftLeft + right),

            OrSecurityRule
            {
                Left: OrSecurityRule { Left: { } leftLeft, Right: ExpandedRolesSecurityRule leftRight },
                Right: ExpandedRolesSecurityRule right
            } when leftRight.CustomExpandType == right.CustomExpandType => leftLeft.Or(leftRight + right),

            OrSecurityRule
                {
                    Left: ExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: ExpandedRolesSecurityRule rightLeft, Right: { } rightRight }
                }
                when left.CustomExpandType == rightLeft.CustomExpandType => (left + rightLeft).Or(rightRight),

            OrSecurityRule
                {
                    Left: ExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: { } rightLeft, Right: ExpandedRolesSecurityRule rightRight }
                }
                when left.CustomExpandType == rightRight.CustomExpandType => (left + rightRight).Or(rightLeft),

            //NonExpandedRolesSecurityRule

            OrSecurityRule { Left: NonExpandedRolesSecurityRule left, Right: NonExpandedRolesSecurityRule right }
                when left.CustomExpandType == right.CustomExpandType => left + right,

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: NonExpandedRolesSecurityRule leftLeft, Right: { } leftRight },
                    Right: NonExpandedRolesSecurityRule right
                }
                when leftLeft.CustomExpandType == right.CustomExpandType => leftRight.Or(leftLeft + right),

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: { } leftLeft, Right: NonExpandedRolesSecurityRule leftRight },
                    Right: NonExpandedRolesSecurityRule right
                }
                when leftRight.CustomExpandType == right.CustomExpandType => leftLeft.Or(leftRight + right),

            OrSecurityRule
                {
                    Left: NonExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: NonExpandedRolesSecurityRule rightLeft, Right: { } rightRight }
                }
                when left.CustomExpandType == rightLeft.CustomExpandType => (left + rightLeft).Or(rightRight),

            OrSecurityRule
                {
                    Left: NonExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: { } rightLeft, Right: NonExpandedRolesSecurityRule rightRight }
                }
                when left.CustomExpandType == rightRight.CustomExpandType => (left + rightRight).Or(rightLeft),

            _ => visitedBase
        };
    }

    protected override DomainSecurityRule Visit(AndSecurityRule baseSecurityRule)
    {
        var visitedBase = base.Visit(baseSecurityRule);

        return visitedBase switch
        {
            AndSecurityRule { Left: { } left, Right: { } right } when left == Disabled => right,

            AndSecurityRule { Left: { } left, Right: { } right } when right == Disabled => left,

            _ => visitedBase
        };
    }

    protected override DomainSecurityRule Visit(NegateSecurityRule baseSecurityRule)
    {
        var visitedBase = base.Visit(baseSecurityRule);

        return visitedBase switch
        {
            NegateSecurityRule { InnerRule: NegateSecurityRule deepNegateRule } => deepNegateRule.InnerRule,
            _ => visitedBase
        };
    }

    DomainSecurityRule ISecurityRuleOptimizer.Optimize(DomainSecurityRule securityRule) =>
        this.Visit(securityRule);
}
