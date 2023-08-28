using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.Security;

/// <summary>
/// Атрибут для отображения объекта (или его свойства)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Property)]
public class ViewDomainObjectAttribute : DomainObjectAccessAttribute
{
    private readonly ReadOnlyCollection<Enum> baseSecondaryOperations;

    private Type[] sourceTypes;

    private ReadOnlyCollection<Enum> editSourceOperations = new ReadOnlyCollection<Enum>(new List<Enum>());

    /// <summary>
    /// Пустой констуктор для кастомной безопасности
    /// </summary>
    public ViewDomainObjectAttribute()
            : this(default(Enum))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции
    /// </summary>
    /// <param name="primarySecurityOperationCode">Операция просмотра</param>
    public ViewDomainObjectAttribute(Enum primarySecurityOperationCode)
            : this(primarySecurityOperationCode, new Enum[0])
    {
    }

    /// <summary>
    /// Констуктор с доступом по операции из View-атрибута типа
    /// </summary>
    /// <param name="viewSecurityType">Доменный тип</param>
    public ViewDomainObjectAttribute(Type viewSecurityType)
            : this(viewSecurityType.GetViewDomainObjectCode(true))
    {
    }

    /// <summary>
    /// Констуктор с доступом по операциям
    /// </summary>
    /// <param name="primarySecurityOperationCode">Операция просмотра</param>
    /// <param name="baseSecondaryOperations">Дополнительные операции для просмотра</param>
    public ViewDomainObjectAttribute(Enum primarySecurityOperationCode, IEnumerable<Enum> baseSecondaryOperations)
            : base(primarySecurityOperationCode)
    {
        if (baseSecondaryOperations == null) throw new ArgumentNullException(nameof(baseSecondaryOperations));

        this.baseSecondaryOperations = baseSecondaryOperations.ToReadOnlyCollection();

        this.IsContext = this.SecurityOperationCode.Maybe(code => code.GetSecurityOperationAttribute().IsContext);

        this.CheckSecondaryOperations(this.baseSecondaryOperations);
    }

    /// <summary>
    /// Дополнительные операции для просмотра
    /// </summary>
    public IEnumerable<Enum> SecondaryOperations => this.baseSecondaryOperations.Concat(this.editSourceOperations).Distinct();

    /// <summary>
    /// Все операции для просмотра объекта
    /// </summary>
    public IEnumerable<Enum> AllOperations => new[] { this.SecurityOperationCode }.Concat(this.SecondaryOperations).Distinct();

    /// <summary>
    /// Типы, для редактирования которых требуется данный объекта (из типов забираются edit-операции)
    /// </summary>
    public Type[] SourceTypes
    {
        get
        {
            return this.sourceTypes.ToArray();
        }

        set
        {
            var operations = value.ToReadOnlyCollection(type => type.GetEditDomainObjectCode(true));

            this.CheckSecondaryOperations(operations);

            this.editSourceOperations = operations;
            this.sourceTypes = value.ToArray();
        }
    }

    private bool IsContext { get; }

    private void CheckSecondaryOperations(IEnumerable<Enum> secondaryOperations)
    {
        if (secondaryOperations == null) throw new ArgumentNullException(nameof(secondaryOperations));

        if (this.IsContext)
        {
            var nonContextOperations = secondaryOperations.Where(operation => !operation.GetSecurityOperationAttribute().IsContext).ToList();

            if (nonContextOperations.Any())
            {
                throw new Exception($"Invalid secondary operations: {nonContextOperations.Join(", ")}. All operations must be context.");
            }
        }
    }
}
