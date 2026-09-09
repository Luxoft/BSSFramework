using Framework.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

/// <summary>
/// Базовый персистентные класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase
{
    private DateTime? createDate;

    private string? createdBy = "";

    private DateTime? modifyDate;

    private string? modifiedBy = "";


    /// <summary>
    /// Дата создания доменного объекта
    /// </summary>
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        protected internal set => this.createDate = value;
    }

    /// <summary>
    /// Дата изменения доменного объекта
    /// </summary>
    public virtual DateTime? ModifyDate
    {
        get => this.modifyDate;
        protected internal set => this.modifyDate = value;
    }

    /// <summary>
    /// Логин сотрудника, изменившего доменный объект
    /// </summary>
    public virtual string? ModifiedBy
    {
        get => this.modifiedBy;
        protected internal set => this.modifiedBy = value;
    }

    /// <summary>
    /// Логин сотрудника, создавшего доменный объект
    /// </summary>
    public virtual string? CreatedBy
    {
        get => this.createdBy;
        protected internal set => this.createdBy = value;
    }
}
