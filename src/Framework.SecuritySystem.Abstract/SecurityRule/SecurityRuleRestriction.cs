using Framework.Core;

namespace Framework.SecuritySystem;

public record SecurityRuleRestriction(DeepEqualsCollection<Type> SecurityContexts)
{
    public static SecurityRuleRestriction Empty { get; } = Create([]);

    public static SecurityRuleRestriction Create(IEnumerable<Type> securityContexts)
    {
        return new SecurityRuleRestriction(DeepEqualsCollection.Create(securityContexts.Distinct()));
    }

    public SecurityRuleRestriction Add<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        return new SecurityRuleRestriction(
            DeepEqualsCollection.Create(this.SecurityContexts.Concat([typeof(TSecurityContext)]).Distinct()));
    }

    public static SecurityRuleRestriction Create<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        return Empty.Add<TSecurityContext>();
    }
}
