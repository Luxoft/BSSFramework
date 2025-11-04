using System.ComponentModel;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain;

/// <summary>
///     Базовый персистентный класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IAuditObject, IVersionObject<long>
{
    private bool active = true;

    private string? createdBy;

    private DateTime? createDate;

    private DateTime? modifyDate;

    private string? modifiedBy;

    private long version;

    /// <summary>
    ///     Дата создания доменного объекта
    /// </summary>
    public virtual DateTime? CreateDate
    {
        get { return this.createDate; }
        protected internal set { this.createDate = value; }
    }

    /// <summary>
    ///     Логин сотрудника, создавшего доменный объект
    /// </summary>
    [SystemProperty]
    public virtual DateTime? ModifyDate
    {
        get { return this.modifyDate; }
        protected internal set { this.modifyDate = value; }
    }

    /// <summary>
    ///     Дата изменения доменного объекта
    /// </summary>
    [SystemProperty]
    public virtual string? ModifiedBy
    {
        get { return this.modifiedBy.TrimNull(); }
        protected internal set { this.modifiedBy = value.TrimNull(); }
    }

    /// <summary>
    ///     Логин сотрудника, изменившего доменный объект
    /// </summary>
    public virtual string? CreatedBy
    {
        get { return this.createdBy.TrimNull(); }
        protected internal set { this.createdBy = value.TrimNull(); }
    }

    /// <summary>
    ///     Признак активности доменного объекта
    /// </summary>
    [DefaultValue(true)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool Active
    {
        get { return this.active; }
        set { this.active = value; }
    }

    [Version]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    public virtual long Version
    {
        get { return this.version; }
        set { this.version = value; }
    }
}
