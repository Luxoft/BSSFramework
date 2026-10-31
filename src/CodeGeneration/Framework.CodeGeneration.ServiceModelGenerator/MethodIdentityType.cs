using System.Linq.Expressions;

using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator;

/// <summary>
/// Identifier of the generated facade method (used for DefaultServiceGeneratePolicy)
/// </summary>
public class MethodIdentityType(string name) : IEquatable<MethodIdentityType>
{
    public readonly string Name = name ?? throw new ArgumentNullException(nameof(name));

    public MethodIdentityType(Expression<Func<MethodIdentityType>> expr)
            : this(expr.GetStaticMemberName())
    {
    }

    public static readonly MethodIdentityType Save = new(() => Save!);

    public static readonly MethodIdentityType SaveMany = new(() => SaveMany!);

    public static readonly MethodIdentityType Update = new(() => Update!);

    public static readonly MethodIdentityType GetWithExtended = new(() => GetWithExtended!);

    public static readonly MethodIdentityType SaveByModel = new(() => SaveByModel!);

    public static readonly MethodIdentityType Remove = new(() => Remove!);

    public static readonly MethodIdentityType RemoveMany = new(() => RemoveMany!);

    public static readonly MethodIdentityType GetPropertyRevisions = new(() => GetPropertyRevisions!);

    public static readonly MethodIdentityType GetPropertyRevisionByDateRange = new(() => GetPropertyRevisionByDateRange!);

    public static readonly MethodIdentityType GetRevisions = new(() => GetRevisions!);

    public static readonly MethodIdentityType GetRevision = new(() => GetRevision!);

    public static readonly MethodIdentityType AddAttachment = new(() => AddAttachment!);

    public static readonly MethodIdentityType GetAttachment = new(() => GetAttachment!);

    public static readonly MethodIdentityType RemoveAttachment = new(() => RemoveAttachment!);

    public static readonly MethodIdentityType GetFileContainer = new(() => GetFileContainer!);

    public static readonly MethodIdentityType IntegrationSave = new(() => IntegrationSave!);

    public static readonly MethodIdentityType IntegrationSaveMany = new(() => IntegrationSaveMany!);

    public static readonly MethodIdentityType IntegrationSaveByModel = new(() => IntegrationSaveByModel!);

    public static readonly MethodIdentityType IntegrationRemove = new(() => IntegrationRemove!);

    public static readonly MethodIdentityType GetChangeModel = new(() => GetChangeModel!);

    public static readonly MethodIdentityType Change = new(() => Change!);

    public static readonly MethodIdentityType GetMassChangeModel = new(() => GetMassChangeModel!);

    public static readonly MethodIdentityType MassChange = new(() => MassChange!);

    public static readonly MethodIdentityType HasAccess = new(() => HasAccess!);

    public static readonly MethodIdentityType CheckAccess = new(() => CheckAccess!);

    public static readonly MethodIdentityType Create = new(() => Create!);

    public static readonly MethodIdentityType GetWithFormat = new(() => GetWithFormat!);

    /// <summary>
    /// Getting an object by identifier (for ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByIdentity = new(() => GetSingleByIdentity!);

    /// <summary>
    /// Getting an object by name (for MainDTOType, requires implementation of <see cref="IVisualIdentityObject"/> )
    /// </summary>
    public static readonly MethodIdentityType GetSingleByName = new(() => GetSingleByName!);

    /// <summary>
    /// Getting an object by code (for MainDTOType, requires implementation of <see cref="ICodeObject"/>)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByCode = new(() => GetSingleByCode!);

    /// <summary>
    /// Getting the full list (for ViewDTOType, disabled for projections by default)
    /// </summary>
    public static readonly MethodIdentityType GetList = new(() => GetList!);

    /// <summary>
    /// Getting a list by filter (for ViewDTOType, disabled for projections by default)
    /// </summary>
    public static readonly MethodIdentityType GetListByFilter = new(() => GetListByFilter!);

    /// <summary>
    /// Getting a list by identifiers (for ViewDTOType, disabled for projections by default)
    /// </summary>
    public static readonly MethodIdentityType GetListByIdents = new(() => GetListByIdents!);

    /// <summary>
    /// Getting the full list by a dependent Security operation (for ViewDTOType, disabled for projections by default, dependent Security operations are required)
    /// </summary>
    public static readonly MethodIdentityType GetListByOperation = new(() => GetListByOperation!);

    /// <summary>
    /// Getting a tree by a dependent Security operation (for ViewDTOType, disabled for projections by default, requires implementation of IHierarchicalPersistentDomainObjectBase and dependent Security operations)
    /// </summary>
    public static readonly MethodIdentityType GetTreeByOperation = new(() => GetTreeByOperation!);

    /// <summary>
    /// Getting an object by filter (only for projections)
    /// </summary>
    public static readonly MethodIdentityType GetSingleByFilter = new(() => GetSingleByFilter!);

    /// <summary>
    /// Getting an OData list (for ViewDTOType)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryString = new(() => GetODataListByQueryString!);

    /// <summary>
    /// Getting an OData list by an additional filter (for ViewDTOType, by default only for projections)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithFilter = new(() => GetODataListByQueryStringWithFilter!);

    /// <summary>
    /// Getting an OData list by a dependent Security operation (for ViewDTOType, by default only for projections, dependent Security operations are required)
    /// </summary>
    public static readonly MethodIdentityType GetODataListByQueryStringWithOperation = new(() => GetODataListByQueryStringWithOperation!);

    /// <summary>
    /// Getting an OData tree by a dependent Security operation (for ViewDTOType, by default only for projections, dependent Security operations are required)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithOperation = new(() => GetODataTreeByQueryStringWithOperation!);

    /// <summary>
    /// Getting an OData tree by a dependent Security operation (for ViewDTOType, by default only for projections, requires implementation of IHierarchicalPersistentDomainObjectBase and dependent Security operations)
    /// </summary>
    public static readonly MethodIdentityType GetODataTreeByQueryStringWithFilter = new(() => GetODataTreeByQueryStringWithFilter!);

    public virtual bool Equals(MethodIdentityType? other) => other is not null && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);

    public override bool Equals(object? obj) => this.Equals(obj as MethodIdentityType);

    public override int GetHashCode() => this.Name.ToLower().GetHashCode();

    public override string ToString() => this.Name;

    public static bool operator ==(MethodIdentity? ident, MethodIdentityType type) => ident is not null && ident.Type == type;

    public static bool operator !=(MethodIdentity? ident, MethodIdentityType type) => !(ident == type);

    public static bool operator ==(MethodIdentityType type, MethodIdentity? ident) => ident is not null && ident.Type == type;

    public static bool operator !=(MethodIdentityType type, MethodIdentity? ident) => !(ident == type);

    public static bool operator ==(MethodIdentityType? fileType, MethodIdentityType? other) =>
        ReferenceEquals(fileType, other)
        || (fileType is not null && fileType.Equals(other));

    public static bool operator !=(MethodIdentityType? fileType, MethodIdentityType? other) => !(fileType == other);
}
