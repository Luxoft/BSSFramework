using System.Diagnostics;

using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
///     Идентификатор (логин) пользователя в системе
/// </summary>
[DebuggerDisplay("{Name}, RunAs={RunAs}")]
[UniqueGroup]
public class Principal : BaseDirectory, IMaster<Permission>
{
    private readonly ICollection<Permission> permissions = new List<Permission>();

    private Principal runAs;

    /// <summary>
    ///     Коллекция пермиссий принципала
    /// </summary>
    public virtual IEnumerable<Permission> Permissions => this.permissions;

    /// <summary>
    ///     Принципал, под которым сейчас работает пользователь
    /// </summary>
    public virtual Principal RunAs
    {
        get => this.runAs;
        set => this.runAs = value;
    }

    ICollection<Permission> IMaster<Permission>.Details => (ICollection<Permission>)this.Permissions;
}
