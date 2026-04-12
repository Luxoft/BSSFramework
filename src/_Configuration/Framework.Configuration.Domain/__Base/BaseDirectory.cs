using Framework.Application.Domain;
using Framework.Core;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Базовый персистентный класс для типов с именем
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    private string name;

    /// <summary>
    /// Название типа
    /// </summary>
    [Required]
    [UniqueElement]
    public virtual string Name
    {
        get => this.name.TrimNull();
        set => this.name = value.TrimNull();
    }

    public override string ToString() => this.Name;
}
