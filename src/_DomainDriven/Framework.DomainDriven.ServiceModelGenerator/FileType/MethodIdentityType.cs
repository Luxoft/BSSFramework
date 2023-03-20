using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModelGenerator;

/// <summary>
/// Идентификатор генерируемого фасадного метода (используется для политики DefaultServiceGeneratePolicy)
/// </summary>
public class MethodIdentityType : IEquatable<MethodIdentityType>
{
    public readonly string Name;

    public MethodIdentityType(Expression<Func<MethodIdentityType>> expr)
            : this(expr.GetStaticMemberName())
    {
    }

    public MethodIdentityType(string name)
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static readonly MethodIdentityType Save = new MethodIdentityType(() => Save);

    public static readonly MethodIdentityType SaveMany = new MethodIdentityType(() => SaveMany);

    public static readonly MethodIdentityType Update = new MethodIdentityType(() => Update);

    public static readonly MethodIdentityType GetWithExtended = new MethodIdentityType(() => GetWithExtended);

    public static readonly MethodIdentityType SaveByModel = new MethodIdentityType(() => SaveByModel);

    public static readonly MethodIdentityType Remove = new MethodIdentityType(() => Remove);

    public static readonly MethodIdentityType RemoveMany = new MethodIdentityType(() => RemoveMany);

    public static readonly MethodIdentityType GetPropertyRevisions = new MethodIdentityType(() => GetPropertyRevisions);

    public static readonly MethodIdentityType GetPropertyRevisionByDateRange = new MethodIdentityType(() => GetPropertyRevisionByDateRange);

    public static readonly MethodIdentityType GetRevisions = new MethodIdentityType(() => GetRevisions);

    public static readonly MethodIdentityType GetRevision = new MethodIdentityType(() => GetRevision);

    public static readonly MethodIdentityType AddAttachment = new MethodIdentityType(() => AddAttachment);

    public static readonly MethodIdentityType GetAttachment = new MethodIdentityType(() => GetAttachment);

    public static readonly MethodIdentityType RemoveAttachment = new MethodIdentityType(() => RemoveAttachment);

    public static readonly MethodIdentityType GetFileContainer = new MethodIdentityType(() => GetFileContainer);

    public static readonly MethodIdentityType IntegrationSave = new MethodIdentityType(() => IntegrationSave);

    public static readonly MethodIdentityType IntegrationSaveMany = new MethodIdentityType(() => IntegrationSaveMany);

    public static readonly MethodIdentityType IntegrationSaveByModel = new MethodIdentityType(() => IntegrationSaveByModel);

    public static readonly MethodIdentityType IntegrationRemove = new MethodIdentityType(() => IntegrationRemove);

    public static readonly MethodIdentityType GetChangeModel = new MethodIdentityType(() => GetChangeModel);

    public static readonly MethodIdentityType Change = new MethodIdentityType(() => Change);

    public static readonly MethodIdentityType GetMassChangeModel = new MethodIdentityType(() => GetMassChangeModel);

    public static readonly MethodIdentityType MassChange = new MethodIdentityType(() => MassChange);

    public static readonly MethodIdentityType HasAccess = new MethodIdentityType(() => HasAccess);

    public static readonly MethodIdentityType CheckAccess = new MethodIdentityType(() => CheckAccess);

    public static readonly MethodIdentityType Create = new MethodIdentityType(() => Create);

    public static readonly MethodIdentityType GetWithFormat = new MethodIdentityType(() => GetWithFormat);

    /// <summary>
    /// Получение объекта по идентификатору (для ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByIdentity = new MethodIdentityType(() => GetSingleByIdentity);

    /// <summary>
    /// Получение объекта по имени (для MainDTOType, требуется реализация <see cref="IVisualIdentityObject"/> )
    /// </summary>
    public static readonly MethodIdentityType GetSingleByName = new MethodIdentityType(() => GetSingleByName);

    /// <summary>
    /// Получение объекта по коду (для MainDTOType, требуется реализация <see cref="ICodeObject{TCode}"/>)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByCode = new MethodIdentityType(() => GetSingleByCode);

    /// <summary>
    /// Получение полного списка (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetList = new MethodIdentityType(() => GetList);

    /// <summary>
    /// Получение списка по фильтру (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetListByFilter = new MethodIdentityType(() => GetListByFilter);

    /// <summary>
    /// Получение списка по идентификаторам (для ViewDTOType, по умолчанию для проекций отключено)
    /// </summary>
    public static readonly MethodIdentityType GetListByIdents = new MethodIdentityType(() => GetListByIdents);

    /// <summary>
    /// Получение полного списка по зависимой Security-операции (для ViewDTOType, по умолчанию для проекций отключено, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetListByOperation = new MethodIdentityType(() => GetListByOperation);

    /// <summary>
    /// Получение дерева по зависимой Security-операции (для ViewDTOType, по умолчанию отключено для проекций, требуются релизация IHierarchicalPersistentDomainObjectBase и зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetTreeByOperation = new MethodIdentityType(() => GetTreeByOperation);

    /// <summary>
    /// Получение объекта по фильтру (только для проекций)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByFilter = new MethodIdentityType(() => GetSingleByFilter);

    /// <summary>
    /// Получение OData-списка (для ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryString = new MethodIdentityType(() => GetODataListByQueryString);

    /// <summary>
    /// Получение OData-списка по дополнительному фильтру (для ViewDTOType, по умолчанию только для проекций)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithFilter = new MethodIdentityType(() => GetODataListByQueryStringWithFilter);

    /// <summary>
    /// Получение OData-списка по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithOperation = new MethodIdentityType(() => GetODataListByQueryStringWithOperation);

    /// <summary>
    /// Получение OData-дерева по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithOperation = new MethodIdentityType(() => GetODataTreeByQueryStringWithOperation);

    /// <summary>
    /// Получение OData-дерева по зависимой Security-операции (для ViewDTOType, по умолчанию только для проекций, требуются релизация IHierarchicalPersistentDomainObjectBase и зависимые Security-операции)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithFilter = new MethodIdentityType(() => GetODataTreeByQueryStringWithFilter);

    public virtual bool Equals(MethodIdentityType other)
    {
        return !object.ReferenceEquals(other, null) && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as MethodIdentityType);
    }

    public override int GetHashCode()
    {
        return this.Name.ToLower().GetHashCode();
    }

    public override string ToString()
    {
        return this.Name;
    }

    public static bool operator ==(MethodIdentity ident, MethodIdentityType type)
    {
        return ident != null && ident.Type == type;
    }
    public static bool operator !=(MethodIdentity ident, MethodIdentityType type)
    {
        return !(ident == type);
    }
    public static bool operator ==(MethodIdentityType type, MethodIdentity ident)
    {
        return ident != null && ident.Type == type;
    }
    public static bool operator !=(MethodIdentityType type, MethodIdentity ident)
    {
        return !(ident == type);
    }

    public static bool operator ==(MethodIdentityType fileType, MethodIdentityType other)
    {
        return object.ReferenceEquals(fileType, other)
               || (!object.ReferenceEquals(fileType, null) && fileType.Equals(other));
    }

    public static bool operator !=(MethodIdentityType fileType, MethodIdentityType other)
    {
        return !(fileType == other);
    }
}
