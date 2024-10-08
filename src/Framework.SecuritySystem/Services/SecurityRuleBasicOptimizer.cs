using Framework.Core;

using static Framework.SecuritySystem.SecurityRule;
using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleBasicOptimizer : SecurityRuleVisitor, ISecurityRuleBasicOptimizer
{
    private readonly IDictionaryCache<DomainSecurityRule, DomainSecurityRule> cache;

    public SecurityRuleBasicOptimizer()
    {
        this.cache = new DictionaryCache<DomainSecurityRule, DomainSecurityRule>(this.Visit).WithLock();
    }

    protected override DomainSecurityRule Visit(OrSecurityRule baseSecurityRule)
    {
        var visitedBase = base.Visit(baseSecurityRule);

        return visitedBase switch
        {
            OrSecurityRule { Left: { } left, Right: { } right }
                when left == Disabled || right == Disabled => Disabled,

            OrSecurityRule { Left: ExpandedRolesSecurityRule left, Right: ExpandedRolesSecurityRule right }
                when left.EqualsCustoms(right) => left + right,

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: ExpandedRolesSecurityRule leftLeft, Right: { } leftRight },
                    Right: ExpandedRolesSecurityRule right
                }
                when leftLeft.EqualsCustoms(right) => leftRight.Or(leftLeft + right),

            OrSecurityRule
            {
                Left: OrSecurityRule { Left: { } leftLeft, Right: ExpandedRolesSecurityRule leftRight },
                Right: ExpandedRolesSecurityRule right
            } when leftRight.EqualsCustoms(right) => leftLeft.Or(leftRight + right),

            OrSecurityRule
                {
                    Left: ExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: ExpandedRolesSecurityRule rightLeft, Right: { } rightRight }
                }
                when left.EqualsCustoms(rightLeft) => (left + rightLeft).Or(rightRight),

            OrSecurityRule
                {
                    Left: ExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: { } rightLeft, Right: ExpandedRolesSecurityRule rightRight }
                }
                when left.EqualsCustoms(rightRight) => (left + rightRight).Or(rightLeft),

            //NonExpandedRolesSecurityRule

            OrSecurityRule { Left: NonExpandedRolesSecurityRule left, Right: NonExpandedRolesSecurityRule right }
                when left.EqualsCustoms(right) => left + right,

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: NonExpandedRolesSecurityRule leftLeft, Right: { } leftRight },
                    Right: NonExpandedRolesSecurityRule right
                }
                when leftLeft.EqualsCustoms(right) => leftRight.Or(leftLeft + right),

            OrSecurityRule
                {
                    Left: OrSecurityRule { Left: { } leftLeft, Right: NonExpandedRolesSecurityRule leftRight },
                    Right: NonExpandedRolesSecurityRule right
                }
                when leftRight.EqualsCustoms(right) => leftLeft.Or(leftRight + right),

            OrSecurityRule
                {
                    Left: NonExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: NonExpandedRolesSecurityRule rightLeft, Right: { } rightRight }
                }
                when left.EqualsCustoms(rightLeft) => (left + rightLeft).Or(rightRight),

            OrSecurityRule
                {
                    Left: NonExpandedRolesSecurityRule left,
                    Right: OrSecurityRule { Left: { } rightLeft, Right: NonExpandedRolesSecurityRule rightRight }
                }
                when left.EqualsCustoms(rightRight) => (left + rightRight).Or(rightLeft),

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

    DomainSecurityRule ISecurityRuleBasicOptimizer.Optimize(DomainSecurityRule securityRule) =>
        this.cache[securityRule];
}
