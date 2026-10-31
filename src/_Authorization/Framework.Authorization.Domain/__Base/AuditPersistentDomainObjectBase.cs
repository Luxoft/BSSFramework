

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

/// <summary>
/// Base persistent class
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase
{
    private DateTime? createDate;

    private string? createdBy = "";

    private DateTime? modifyDate;

    private string? modifiedBy = "";


    /// <summary>
    /// Domain object creation date
    /// </summary>
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        protected internal set => this.createDate = value;
    }

    /// <summary>
    /// Domain object modification date
    /// </summary>
    public virtual DateTime? ModifyDate
    {
        get => this.modifyDate;
        protected internal set => this.modifyDate = value;
    }

    /// <summary>
    /// Login of the employee who modified the domain object
    /// </summary>
    public virtual string? ModifiedBy
    {
        get => this.modifiedBy;
        protected internal set => this.modifiedBy = value;
    }

    /// <summary>
    /// Login of the employee who created the domain object
    /// </summary>
    public virtual string? CreatedBy
    {
        get => this.createdBy;
        protected internal set => this.createdBy = value;
    }
}
