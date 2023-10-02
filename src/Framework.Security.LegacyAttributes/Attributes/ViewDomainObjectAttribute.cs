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
    private readonly ReadOnlyCollection<SecurityOperation> baseSecondaryOperations = new ReadOnlyCollection<SecurityOperation>(new List<SecurityOperation>());

    private Type[] sourceTypes;

    private ReadOnlyCollection<SecurityOperation> editSourceOperations = new ReadOnlyCollection<SecurityOperation>(new List<SecurityOperation>());

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

    public ViewDomainObjectAttribute(SecurityOperation primarySecurityOperation, params SecurityOperation[] baseSecondaryOperations)
        : base(primarySecurityOperation)
    {
        if (baseSecondaryOperations == null) throw new ArgumentNullException(nameof(baseSecondaryOperations));

        this.baseSecondaryOperations = baseSecondaryOperations.ToReadOnlyCollection();

        this.CheckSecondaryOperations(this.baseSecondaryOperations);
    }

    /// <summary>
    /// Дополнительные операции для просмотра
    /// </summary>
    public IEnumerable<SecurityOperation> SecondaryOperations => this.baseSecondaryOperations.Concat(this.editSourceOperations).Distinct();

    /// <summary>
    /// Все операции для просмотра объекта
    /// </summary>
    public IEnumerable<SecurityOperation> AllOperations => new[] { this.SecurityOperation }.Concat(this.SecondaryOperations).Distinct();

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

    private void CheckSecondaryOperations(IEnumerable<SecurityOperation> secondaryOperations)
    {
        if (secondaryOperations == null) throw new ArgumentNullException(nameof(secondaryOperations));

        if (this.HasContext)
        {
            var nonContextOperations = secondaryOperations.Where(operation => !(operation is SecurityOperation)).ToList();

            if (nonContextOperations.Any())
            {
                throw new Exception($"Invalid secondary operations: {nonContextOperations.Join(", ")}. All operations must be context.");
            }
        }
    }
}
