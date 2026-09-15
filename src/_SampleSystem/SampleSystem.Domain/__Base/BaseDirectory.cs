using Framework.Application.Domain;
using Framework.BLL.Domain.Attributes;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace SampleSystem.Domain;

/// <summary>
///     Базовый персистентный класс для типов с именем
/// </summary>
public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
{
    /// <summary>
    ///     Название типа
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
