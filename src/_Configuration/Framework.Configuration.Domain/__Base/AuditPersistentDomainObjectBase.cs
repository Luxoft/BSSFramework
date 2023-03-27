using System.ComponentModel;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Configuration.Domain;

/// <summary>
/// Базовый персистентные класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IDefaultAuditPersistentDomainObjectBase
{
    private bool active = true;

    private string createdBy;
    private DateTime? createDate;

    private DateTime? modifyDate;
    private string modifiedBy;

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
        get { return this.createDate; }
        internal protected set { this.createDate = value; }
    }

    /// <summary>
    /// Дата изменения доменного объекта
    /// </summary>
    [SystemProperty]
    [NotAuditedProperty]
    public virtual DateTime? ModifyDate
    {
        get { return this.modifyDate; }
        protected internal set { this.modifyDate = value; }
    }

    /// <summary>
    /// Логин сотрудника, изменившего доменный объект
    /// </summary>
    [SystemProperty]
    [NotAuditedProperty]
    public virtual string ModifiedBy
    {
        get { return this.modifiedBy.TrimNull(); }
        protected internal set { this.modifiedBy = value.TrimNull(); }
    }


    /// <summary>
    /// Логин сотрудника, создавшего доменный объект
    /// </summary>
    [NotAuditedProperty]
    public virtual string CreatedBy
    {
        get { return this.createdBy.TrimNull(); }
        internal protected set { this.createdBy = value.TrimNull(); }
    }

    /// <summary>
    /// Признак активности доменного объекта
    /// </summary>
    [DefaultValue(true)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool Active
    {
        get { return this.active; }
        set { this.active = value; }
    }
}
