using System.Collections.ObjectModel;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

/// <summary>
/// Атрибут для отображения объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Property)]
public class ViewDomainObjectAttribute : DomainObjectAccessAttribute
{
    private readonly ReadOnlyCollection<SecurityRule> baseSecondaryRules = new ReadOnlyCollection<SecurityRule>(new List<SecurityRule>());

    private Type[] sourceTypes;

    private ReadOnlyCollection<SecurityRule> editSourceRules = new ReadOnlyCollection<SecurityRule>(new List<SecurityRule>());

    /// <summary>
    /// Пустой констуктор для кастомной безопасности
    /// </summary>
    public ViewDomainObjectAttribute()
        : base(null)
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    /// <param name="primarySecurityRule">Операция просмотра</param>
    public ViewDomainObjectAttribute(Type securityRuleType, string primarySecurityRule)
        : this(securityRuleType, primarySecurityRule, new string[0])
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из View-атрибута типа
    /// </summary>
    /// <param name="viewSecurityType">Доменный тип</param>
    public ViewDomainObjectAttribute(Type viewSecurityType)
        : base(viewSecurityType.GetViewSecurityRule(true))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операциям
    /// </summary>
    /// <param name="primarySecurityRule">Операция просмотра</param>
    /// <param name="baseSecondaryRules">Дополнительные операции для просмотра</param>
    public ViewDomainObjectAttribute(
        Type securityRuleType,
        string primarySecurityRule,
        params string[] baseSecondaryRules)
        : this(
            securityRuleType.Maybe(v => v.GetSecurityRule(primarySecurityRule)),
            baseSecondaryRules.ToArray(v => securityRuleType.GetSecurityRule(v)))
    {
    }

    public ViewDomainObjectAttribute(SecurityRule primarySecurityRule, params SecurityRule[] baseSecondaryRules)
        : base(primarySecurityRule)
    {
        if (baseSecondaryRules == null) throw new ArgumentNullException(nameof(baseSecondaryRules));

        this.baseSecondaryRules = baseSecondaryRules.ToReadOnlyCollection();

        this.CheckSecondaryRules(this.baseSecondaryRules);
    }

    /// <summary>
    /// Дополнительные операции для просмотра
    /// </summary>
    public IEnumerable<SecurityRule> SecondaryRules => this.baseSecondaryRules.Concat(this.editSourceRules).Distinct();

    /// <summary>
    /// Все операции для просмотра объекта
    /// </summary>
    public IEnumerable<SecurityRule> AllRules => new[] { this.SecurityRule }.Concat(this.SecondaryRules).Distinct();

    /// <summary>
    /// Типы, для редактирования которых требуется данный объекта (из типов забираются edit-операции)
    /// </summary>
    public Type[] SourceTypes
    {
        get { return this.sourceTypes.ToArray(); }
        set
        {
            var rules = value.ToReadOnlyCollection(type => type.GetEditSecurityRule(true));

            this.CheckSecondaryRules(rules);

            this.editSourceRules = rules;
            this.sourceTypes = value.ToArray();
        }
    }

    private void CheckSecondaryRules(IEnumerable<SecurityRule> secondaryRules)
    {
        if (secondaryRules == null) throw new ArgumentNullException(nameof(secondaryRules));

        if (this.HasContext)
        {
            var nonContextRules = secondaryRules.Where(rule => !(rule is SecurityRule)).ToList();

            if (nonContextRules.Any())
            {
                throw new Exception($"Invalid secondary rules: {nonContextRules.Join(", ")}. All rules must be context.");
            }
        }
    }
}
