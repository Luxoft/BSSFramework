using Framework.Core;
using Framework.Database.Mapping;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Базовый персистентные класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase
{
    private DateTime? createDate;

    private string? createdBy = "";

    private DateTime? modifyDate;

    private string? modifiedBy = "";

    #region Constructor

    protected AuditPersistentDomainObjectBase()
    {

    }

    protected AuditPersistentDomainObjectBase(Guid id)
            : base(id)
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
        protected internal set => this.createDate = value;
    }

    /// <summary>
    /// Логин сотрудника, создавшего доменный объект
    /// </summary>
    [NotAuditedProperty]
    public virtual string? CreatedBy
    {
        get => this.createdBy;
        protected internal set => this.createdBy = value;
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
        get => this.modifiedBy;
        protected internal set => this.modifiedBy = value;
    }
}
