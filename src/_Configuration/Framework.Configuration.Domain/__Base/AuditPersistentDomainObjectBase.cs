using Framework.Database.Mapping;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Base persistent class
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
    /// Domain object creation date
    /// </summary>
    [NotAuditedProperty]
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        protected internal set => this.createDate = value;
    }

    /// <summary>
    /// Login of the employee who created the domain object
    /// </summary>
    [NotAuditedProperty]
    public virtual string? CreatedBy
    {
        get => this.createdBy;
        protected internal set => this.createdBy = value;
    }

    /// <summary>
    /// Domain object modification date
    /// </summary>
    [NotAuditedProperty]
    public virtual DateTime? ModifyDate
    {
        get => this.modifyDate;
        protected internal set => this.modifyDate = value;
    }

    /// <summary>
    /// Login of the employee who modified the domain object
    /// </summary>
    [NotAuditedProperty]
    public virtual string? ModifiedBy
    {
        get => this.modifiedBy;
        protected internal set => this.modifiedBy = value;
    }
}
