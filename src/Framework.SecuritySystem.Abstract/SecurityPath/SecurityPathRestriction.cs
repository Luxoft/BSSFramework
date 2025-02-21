using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public record SecurityPathRestriction(
    DeepEqualsCollection<SecurityContextRestriction>? SecurityContextRestrictions,
    DeepEqualsCollection<Type> ConditionFactoryTypes,
    DeepEqualsCollection<RelativeConditionInfo> RelativeConditions,
    bool ApplyBasePath)
{
    public IEnumerable<Type>? SecurityContextTypes => this.SecurityContextRestrictions?.Select(v => v.Type);

    public static SecurityPathRestriction Default { get; } = new(null, Array.Empty<Type>(), Array.Empty<RelativeConditionInfo>(), true);

    public static SecurityPathRestriction Empty { get; } = new(
        Array.Empty<SecurityContextRestriction>(),
        Array.Empty<Type>(),
        Array.Empty<RelativeConditionInfo>(),
        false);

    public SecurityPathRestriction Add<TSecurityContext>(bool required = false, string? key = null)
        where TSecurityContext : ISecurityContext =>
        this with
        {
            SecurityContextRestrictions = this.SecurityContextRestrictions
                                              .EmptyIfNull()
                                              .Concat(
                                                  new[]
                                                  {
                                                      new SecurityContextRestriction(typeof(TSecurityContext), required, key)
                                                  }.Distinct())
                                              .ToArray()
        };

    public SecurityPathRestriction AddRelativeCondition<TDomainObject>(Expression<Func<TDomainObject, bool>> condition) =>

        this with
        {
            RelativeConditions = this.RelativeConditions.Concat([new RelativeConditionInfo<TDomainObject>(condition)]).ToArray()
        };

    public SecurityPathRestriction AddConditionFactory(Type conditionFactoryType) =>

        this with { ConditionFactoryTypes = this.ConditionFactoryTypes.Concat([conditionFactoryType]).ToArray() };

    public static SecurityPathRestriction Create<TSecurityContext>(bool required = false, string? key = null)
        where TSecurityContext : ISecurityContext => Default.Add<TSecurityContext>(required, key);

    public static SecurityPathRestriction Create<TDomainObject>(Expression<Func<TDomainObject, bool>> condition) =>
        Default.AddRelativeCondition(condition);
}
