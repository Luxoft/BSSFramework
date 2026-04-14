using Framework.Application.Domain;
using Framework.Core;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

/// <summary>
/// Базовый персистентный класс для типов с именем
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    /// <summary>
    /// Название типа
    /// </summary>
    private string name;

    [Required]
    [UniqueElement]
    public virtual string Name
    {
        get => this.name.TrimNull();
        set => this.name = value.TrimNull();
    }

    public override string ToString() => this.Name;
}
