#nullable enable

using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.ExpressionComparers;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(DeepEqualsCollection<Type>? SecurityContextTypes, DeepEqualsCollection<Type> ConditionFactoryTypes)
{
    public SecurityPathRestriction(IEnumerable<Type>? securityContexts, IEnumerable<Type> conditionFactoryTypes)
        : this(
            securityContexts == null ? null : DeepEqualsCollection.Create(securityContexts),
            DeepEqualsCollection.Create<Type>(conditionFactoryTypes))
    {
    }

    public static SecurityPathRestriction Empty { get; } = new(null, Array.Empty<Type>());

    public SecurityPathRestriction Add<TSecurityContext>()
        where TSecurityContext : ISecurityContext =>
        new(this.SecurityContextTypes.EmptyIfNull().Concat(new[] { typeof(TSecurityContext) }.Distinct()), this.ConditionFactoryTypes);

    public SecurityPathRestriction AddCondition(Type conditionFactoryType) =>
        new(this.SecurityContextTypes, this.ConditionFactoryTypes.Concat([conditionFactoryType]));

    public static SecurityPathRestriction Create<TSecurityContext>()
        where TSecurityContext : ISecurityContext => Empty.Add<TSecurityContext>();
}
