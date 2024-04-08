using System.Diagnostics;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.Domain;

/// <summary>
///     Идентификатор (логин) пользователя в системе
/// </summary>
[DomainType("{fa27cd64-c5e6-4356-9efa-a35b00ff69dd}")]
[BLLViewRole]
[BLLSaveRole]
[BLLRemoveRole]
[DebuggerDisplay("{Name}, RunAs={RunAs}")]
public class Principal : BaseDirectory, IMaster<Permission>, IPrincipal<Guid>
{
    private readonly ICollection<Permission> permissions = new List<Permission>();

    private Principal runAs;

    private Guid? externalId;

    /// <summary>
    /// Идентификатор сотрудника, использующего эту учётную запись.
    /// </summary>
    public virtual Guid? ExternalId
    {
        get { return this.externalId; }
        set { this.externalId = value; }
    }

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get { return base.Active; }
        set { base.Active = value; }
    }

    /// <summary>
    ///     Коллекция пермиссий принципала
    /// </summary>
    public virtual IEnumerable<Permission> Permissions
    {
        get { return this.permissions; }
    }

    /// <summary>
    ///     Принципал, под которым сейчас работает пользователь
    /// </summary>
    public virtual Principal RunAs
    {
        get { return this.runAs; }
        set { this.runAs = value; }
    }

    ICollection<Permission> IMaster<Permission>.Details
    {
        get { return (ICollection<Permission>)this.Permissions; }
    }

    IEnumerable<IPermission<Guid>> IPrincipal<Guid>.Permissions
    {
        get { return this.Permissions; }
    }
}
