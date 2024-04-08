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
    private readonly ReadOnlyCollection<SecurityRule> baseSecondaryOperations = new ReadOnlyCollection<SecurityRule>(new List<SecurityRule>());

    private Type[] sourceTypes;

    private ReadOnlyCollection<SecurityRule> editSourceOperations = new ReadOnlyCollection<SecurityRule>(new List<SecurityRule>());

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
    /// <param name="primarySecurityOperation">Операция просмотра</param>
    public ViewDomainObjectAttribute(Type securityOperationType, string primarySecurityOperation)
        : this(securityOperationType, primarySecurityOperation, new string[0])
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из View-атрибута типа
    /// </summary>
    /// <param name="viewSecurityType">Доменный тип</param>
    public ViewDomainObjectAttribute(Type viewSecurityType)
        : base(viewSecurityType.GetViewSecurityOperation(true))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операциям
    /// </summary>
    /// <param name="primarySecurityOperation">Операция просмотра</param>
    /// <param name="baseSecondaryOperations">Дополнительные операции для просмотра</param>
    public ViewDomainObjectAttribute(
        Type securityOperationType,
        string primarySecurityOperation,
        params string[] baseSecondaryOperations)
        : this(
            securityOperationType.Maybe(v => v.GetSecurityOperation(primarySecurityOperation)),
            baseSecondaryOperations.ToArray(v => securityOperationType.GetSecurityOperation(v)))
    {
    }

    public ViewDomainObjectAttribute(SecurityRule primarySecurityOperation, params SecurityRule[] baseSecondaryOperations)
        : base(primarySecurityOperation)
    {
        if (baseSecondaryOperations == null) throw new ArgumentNullException(nameof(baseSecondaryOperations));

        this.baseSecondaryOperations = baseSecondaryOperations.ToReadOnlyCollection();

        this.CheckSecondaryOperations(this.baseSecondaryOperations);
    }

    /// <summary>
    /// Дополнительные операции для просмотра
    /// </summary>
    public IEnumerable<SecurityRule> SecondaryOperations => this.baseSecondaryOperations.Concat(this.editSourceOperations).Distinct();

    /// <summary>
    /// Все операции для просмотра объекта
    /// </summary>
    public IEnumerable<SecurityRule> AllOperations => new[] { this.SecurityRule }.Concat(this.SecondaryOperations).Distinct();

    /// <summary>
    /// Типы, для редактирования которых требуется данный объекта (из типов забираются edit-операции)
    /// </summary>
    public Type[] SourceTypes
    {
        get { return this.sourceTypes.ToArray(); }
        set
        {
            var operations = value.ToReadOnlyCollection(type => type.GetEditSecurityOperation(true));

            this.CheckSecondaryOperations(operations);

            this.editSourceOperations = operations;
            this.sourceTypes = value.ToArray();
        }
    }

    private void CheckSecondaryOperations(IEnumerable<SecurityRule> secondaryOperations)
    {
        if (secondaryOperations == null) throw new ArgumentNullException(nameof(secondaryOperations));

        if (this.HasContext)
        {
            var nonContextOperations = secondaryOperations.Where(operation => !(operation is SecurityRule)).ToList();

            if (nonContextOperations.Any())
            {
                throw new Exception($"Invalid secondary operations: {nonContextOperations.Join(", ")}. All operations must be context.");
            }
        }
    }
}
