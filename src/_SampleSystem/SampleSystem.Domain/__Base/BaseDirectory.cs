using Framework.Application.Domain;
using Framework.BLL.Domain.Attributes;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace SampleSystem.Domain;

/// <summary>
///     Base persistent class for named types
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    /// <summary>
    ///     Type name
    /// </summary>
    private string name = "";

    [VisualIdentity]
    [Required]
    [UniqueElement]
    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }

    public override string ToString() => this.Name;
}
