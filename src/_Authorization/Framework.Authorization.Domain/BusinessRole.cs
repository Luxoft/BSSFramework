using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Набор секьюрных операций, который выдается принципалу вместе с контекcтом их применения
/// </summary>
[UniqueGroup]
public class BusinessRole : BaseDirectory
{
    private readonly ICollection<Permission> permissions = new List<Permission>();

    private string description;

    /// <summary>
    /// Коллекция пермиссий принципалов, выданных по одной бизнес-роль
    /// </summary>
    [DetailRole(false)]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual IEnumerable<Permission> Permissions => this.permissions;

    /// <summary>
    /// Описание бизнес-роли
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual string Description
    {
        get => this.description.TrimNull();
        set => this.description = value.TrimNull();
    }
}
