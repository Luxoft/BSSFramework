using Framework.Application.Domain;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Base persistent class for named types
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    private string name = "";

    /// <summary>
    /// Type name
    /// </summary>
    [Required]
    [UniqueElement]
    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }

    public override string ToString() => this.Name;
}
