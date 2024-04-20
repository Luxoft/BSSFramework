using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(DeepEqualsCollection<Type> SecurityContexts, DeepEqualsCollection<LambdaExpression> Conditions)
{
    public SecurityPathRestriction(IEnumerable<Type> securityContexts, IEnumerable<LambdaExpression> conditions)
        : this(
            DeepEqualsCollection.Create(securityContexts),
            DeepEqualsCollection.Create<LambdaExpression>(conditions, ExpressionComparer.Value))
    {
    }

    public static SecurityPathRestriction Empty { get; } = new([], Array.Empty<LambdaExpression>());

    public SecurityPathRestriction Add<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        return new SecurityPathRestriction(
            this.SecurityContexts.Concat(this.SecurityContexts.Concat([typeof(TSecurityContext)]).Distinct()),
            this.Conditions);
    }

    public SecurityPathRestriction Add<TDomainObject>(Expression<Func<TDomainObject, bool>> condition)
    {
        return new SecurityPathRestriction(
            this.SecurityContexts,
            this.Conditions.Concat(new[] { condition }));
    }

    public static SecurityPathRestriction Create<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        return Empty.Add<TSecurityContext>();
    }
}
