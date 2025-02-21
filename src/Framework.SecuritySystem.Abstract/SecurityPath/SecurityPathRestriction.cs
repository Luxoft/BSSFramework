using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

/// <summary>
/// Структура для описания дополнительных ограничений на SecurityPath, связывается с ролью.
/// </summary>
/// <param name="SecurityContextRestrictions"></param>
/// <param name="ConditionFactoryTypes"></param>
/// <param name="RelativeConditions"></param>
/// <param name="ApplyBasePath">Применение базового SecurityPath от доменного объекта</param>
public record SecurityPathRestriction(
    DeepEqualsCollection<SecurityContextRestriction>? SecurityContextRestrictions,
    DeepEqualsCollection<Type> ConditionFactoryTypes,
    DeepEqualsCollection<RelativeConditionInfo> RelativeConditions,
    bool ApplyBasePath)
{
    public IEnumerable<Type>? SecurityContextTypes => this.SecurityContextRestrictions?.Select(v => v.Type);

    /// <summary>
    /// Ограничения по умолчанию для ролей (доступны все типы контекстов, базовый SecurityPath применяется)
    /// </summary>
    public static SecurityPathRestriction Default { get; } = new(null, Array.Empty<Type>(), Array.Empty<RelativeConditionInfo>(), true);

    /// <summary>
    /// Ограничения для базовых ролей 'Administrator' и 'SystemIntegration' (запрещены все контексты и базоый SecurityPath не применяются)
    /// </summary>
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
