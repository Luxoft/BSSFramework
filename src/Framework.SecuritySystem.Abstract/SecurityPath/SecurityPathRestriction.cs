using Framework.Core;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(
    DeepEqualsCollection<SecurityContextRestriction>? SecurityContextRestrictions,
    DeepEqualsCollection<Type> ConditionFactoryTypes)
{
    public SecurityPathRestriction(IEnumerable<SecurityContextRestriction>? securityContexts, IEnumerable<Type> conditionFactoryTypes)
        : this(
            securityContexts == null ? null : DeepEqualsCollection.Create(securityContexts),
            DeepEqualsCollection.Create(conditionFactoryTypes))
    {
    }

    public IEnumerable<Type>? SecurityContextTypes => this.SecurityContextRestrictions?.Select(v => v.Type);

    public static SecurityPathRestriction Empty { get; } = new(null, Array.Empty<Type>());

    public SecurityPathRestriction Add<TSecurityContext>(bool required = false)
        where TSecurityContext : ISecurityContext =>
        new(
            this.SecurityContextRestrictions.EmptyIfNull()
                .Concat(new[] { new SecurityContextRestriction(typeof(TSecurityContext), required) }.Distinct()),
            this.ConditionFactoryTypes);

    public SecurityPathRestriction AddCondition(Type conditionFactoryType) =>
        new(this.SecurityContextRestrictions, this.ConditionFactoryTypes.Concat([conditionFactoryType]));

    public static SecurityPathRestriction Create<TSecurityContext>(bool required = false)
        where TSecurityContext : ISecurityContext => Empty.Add<TSecurityContext>(required);
}
