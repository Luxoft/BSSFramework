using Framework.Application.Domain;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

/// <summary>
/// Base persistent class for named types
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    /// <summary>
    /// Type name
    /// </summary>
    private string name = "";

    [Required]
    [UniqueElement]
    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }

    public override string ToString() => this.Name;
}
