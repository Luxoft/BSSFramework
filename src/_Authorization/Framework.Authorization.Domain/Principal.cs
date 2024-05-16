using System.Diagnostics;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.Domain;

/// <summary>
///     Идентификатор (логин) пользователя в системе
/// </summary>
[DomainType("{fa27cd64-c5e6-4356-9efa-a35b00ff69dd}")]
[BLLViewRole]
[BLLSaveRole]
[BLLRemoveRole]
[DebuggerDisplay("{Name}, RunAs={RunAs}")]
[UniqueGroup]
public class Principal : BaseDirectory, IMaster<Permission>, IPrincipal<Guid>
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

    IEnumerable<IPermission<Guid>> IPrincipal<Guid>.Permissions => this.Permissions;
}
