using System.ComponentModel;

using Framework.BLL.Domain.Serialization;
using Framework.Core;
using Framework.Database;
using Framework.Database.Mapping;

namespace Framework.Configuration.Domain;

/// <summary>
/// Базовый персистентные класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IAuditObject
{
    private bool active = true;

    private string? createdBy;
    private DateTime? createDate;

    private DateTime? modifyDate;
    private string? modifiedBy;

    #region Constructor

    protected AuditPersistentDomainObjectBase()
    {

    }

    protected AuditPersistentDomainObjectBase(Guid id)
            : base (id)
    {
    }

    #endregion

    /// <summary>
    /// Дата создания доменного объекта
    /// </summary>
    [NotAuditedProperty]
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        internal protected set => this.createDate = value;
    }

    /// <summary>
    /// Дата изменения доменного объекта
    /// </summary>
    [NotAuditedProperty]
    public virtual DateTime? ModifyDate
    {
        get => this.modifyDate;
        protected internal set => this.modifyDate = value;
    }

    /// <summary>
    /// Логин сотрудника, изменившего доменный объект
    /// </summary>
    [NotAuditedProperty]
    public virtual string? ModifiedBy
    {
        get => this.modifiedBy.TrimNull();
        protected internal set => this.modifiedBy = value.TrimNull();
    }


    /// <summary>
    /// Логин сотрудника, создавшего доменный объект
    /// </summary>
    [NotAuditedProperty]
    public virtual string? CreatedBy
    {
        get => this.createdBy.TrimNull();
        internal protected set => this.createdBy = value.TrimNull();
    }

    /// <summary>
    /// Признак активности доменного объекта
    /// </summary>
    [DefaultValue(true)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool Active
    {
        get => this.active;
        set => this.active = value;
    }
}
