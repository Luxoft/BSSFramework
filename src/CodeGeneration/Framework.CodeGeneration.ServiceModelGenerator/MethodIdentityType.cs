using System.Linq.Expressions;

using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator;

/// <summary>
/// Идентификатор генерируемого фасадного метода (используется для политики DefaultServiceGeneratePolicy)
/// </summary>
public class MethodIdentityType(string name) : IEquatable<MethodIdentityType>
{
    public readonly string Name = name ?? throw new ArgumentNullException(nameof(name));

    public MethodIdentityType(Expression<Func<MethodIdentityType>> expr)
            : this(expr.GetStaticMemberName())
    {
    }

    public static readonly MethodIdentityType Save = new(() => Save);

    public static readonly MethodIdentityType SaveMany = new(() => SaveMany);

    public static readonly MethodIdentityType Update = new(() => Update);

    public static readonly MethodIdentityType GetWithExtended = new(() => GetWithExtended);

    public static readonly MethodIdentityType SaveByModel = new(() => SaveByModel);

    public static readonly MethodIdentityType Remove = new(() => Remove);

    public static readonly MethodIdentityType RemoveMany = new(() => RemoveMany);

    public static readonly MethodIdentityType GetPropertyRevisions = new(() => GetPropertyRevisions);

    public static readonly MethodIdentityType GetPropertyRevisionByDateRange = new(() => GetPropertyRevisionByDateRange);

    public static readonly MethodIdentityType GetRevisions = new(() => GetRevisions);

    public static readonly MethodIdentityType GetRevision = new(() => GetRevision);

    public static readonly MethodIdentityType AddAttachment = new(() => AddAttachment);

    public static readonly MethodIdentityType GetAttachment = new(() => GetAttachment);

    public static readonly MethodIdentityType RemoveAttachment = new(() => RemoveAttachment);

    public static readonly MethodIdentityType GetFileContainer = new(() => GetFileContainer);

    public static readonly MethodIdentityType IntegrationSave = new(() => IntegrationSave);

    public static readonly MethodIdentityType IntegrationSaveMany = new(() => IntegrationSaveMany);

    public static readonly MethodIdentityType IntegrationSaveByModel = new(() => IntegrationSaveByModel);

    public static readonly MethodIdentityType IntegrationRemove = new(() => IntegrationRemove);

    public static readonly MethodIdentityType GetChangeModel = new(() => GetChangeModel);

    public static readonly MethodIdentityType Change = new(() => Change);

    public static readonly MethodIdentityType GetMassChangeModel = new(() => GetMassChangeModel);

    public static readonly MethodIdentityType MassChange = new(() => MassChange);

    public static readonly MethodIdentityType HasAccess = new(() => HasAccess);

    public static readonly MethodIdentityType CheckAccess = new(() => CheckAccess);

    public static readonly MethodIdentityType Create = new(() => Create);

    public static readonly MethodIdentityType GetWithFormat = new(() => GetWithFormat);

    /// <summary>
    /// Получение объекта по идентификатору (для ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByIdentity = new(() => GetSingleByIdentity);

    /// <summary>
    /// Получение объекта по имени (для MainDTOType, требуется реализация <see cref="IVisualIdentityObject"/> )
    /// </summary>
    public static readonly MethodIdentityType GetSingleByName = new(() => GetSingleByName);

    /// <summary>
    /// Получение объекта по коду (для MainDTOType, требуется реализация <see cref="ICodeObject"/>)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByCode = new(() => GetSingleByCode);

    /// <summary>
    /// Получение полного списка (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetList = new(() => GetList);

    /// <summary>
    /// Получение списка по фильтру (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetListByFilter = new(() => GetListByFilter);

    /// <summary>
    /// Получение списка по идентификаторам (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetListByIdents = new(() => GetListByIdents);

    /// <summary>
    /// Получение полного списка по зависимой Security-операции (для ViewDTOType, по умолчанию для проекций отключено, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetListByOperation = new(() => GetListByOperation);

    /// <summary>
    /// Получение дерева по зависимой Security-операции (для ViewDTOType, по умолчанию отключено для проекций, требуются релизация IHierarchicalPersistentDomainObjectBase и зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetTreeByOperation = new(() => GetTreeByOperation);

    /// <summary>
    /// Получение объекта по фильтру (только для проекций)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByFilter = new(() => GetSingleByFilter);

    /// <summary>
    /// Получение OData-списка (для ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryString = new(() => GetODataListByQueryString);

    /// <summary>
    /// Получение OData-списка по дополнительному фильтру (для ViewDTOType, по умолчанию только для проекций)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithFilter = new(() => GetODataListByQueryStringWithFilter);

    /// <summary>
    /// Получение OData-списка по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithOperation = new(() => GetODataListByQueryStringWithOperation);

    /// <summary>
    /// Получение OData-дерева по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithOperation = new(() => GetODataTreeByQueryStringWithOperation);

    /// <summary>
    /// Получение OData-дерева по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются релизация IHierarchicalPersistentDomainObjectBase и зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithFilter = new(() => GetODataTreeByQueryStringWithFilter);

    public virtual bool Equals(MethodIdentityType other) => !ReferenceEquals(other, null) && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);

    public override bool Equals(object obj) => this.Equals(obj as MethodIdentityType);

    public override int GetHashCode() => this.Name.ToLower().GetHashCode();

    public override string ToString() => this.Name;

    public static bool operator ==(MethodIdentity ident, MethodIdentityType type) => ident != null && ident.Type == type;

    public static bool operator !=(MethodIdentity ident, MethodIdentityType type) => !(ident == type);

    public static bool operator ==(MethodIdentityType type, MethodIdentity ident) => ident != null && ident.Type == type;

    public static bool operator !=(MethodIdentityType type, MethodIdentity ident) => !(ident == type);

    public static bool operator ==(MethodIdentityType fileType, MethodIdentityType other) =>
        ReferenceEquals(fileType, other)
        || (!ReferenceEquals(fileType, null) && fileType.Equals(other));

    public static bool operator !=(MethodIdentityType fileType, MethodIdentityType other) => !(fileType == other);
}
