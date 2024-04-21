#nullable enable

using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(DeepEqualsCollection<Type>? SecurityContexts, DeepEqualsCollection<LambdaExpression> Conditions)
{
    public SecurityPathRestriction(IEnumerable<Type>? securityContexts, IEnumerable<LambdaExpression> conditions)
        : this(
            securityContexts == null ? null : DeepEqualsCollection.Create(securityContexts),
            DeepEqualsCollection.Create<LambdaExpression>(conditions, ExpressionComparer.Value))
    {
    }

    public static SecurityPathRestriction Empty { get; } = new(null, Array.Empty<LambdaExpression>());

    public SecurityPathRestriction Add<TSecurityContext>()
        where TSecurityContext : ISecurityContext =>
        new(this.SecurityContexts.EmptyIfNull().Concat(new[] { typeof(TSecurityContext) }.Distinct()), this.Conditions);

    public SecurityPathRestriction Add<TDomainObject>(Expression<Func<TDomainObject, bool>> condition) =>
        new(this.SecurityContexts, this.Conditions.Concat(new[] { condition }));

    public static SecurityPathRestriction Create<TSecurityContext>()
        where TSecurityContext : ISecurityContext => Empty.Add<TSecurityContext>();

    public static SecurityPathRestriction Create<TDomainObject>(Expression<Func<TDomainObject, bool>> condition)
        => Empty.Add(condition);
}
