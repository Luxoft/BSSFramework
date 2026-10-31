using Framework.Restriction;
using Framework.Validation.Attributes;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Model for forcing a manual event call for an object
/// </summary>
public class DomainTypeEventModel : DomainObjectBase
{
    /// <summary>
    /// Called operation
    /// </summary>
    [Required]
    public DomainTypeEventOperation Operation { get; set; } = null!;

    /// <summary>
    /// Object ident
    /// </summary>
    [Required]
    [AnyElementsValidator]
    public List<Guid> DomainObjectIdents { get; set; } = [];

    /// <summary>
    /// Revision
    /// </summary>
    public long? Revision { get; set; }
}
