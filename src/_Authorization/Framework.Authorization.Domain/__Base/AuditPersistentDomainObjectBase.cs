using Framework.Core;
using Framework.Database;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

/// <summary>
/// Базовый персистентные класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IAuditObject
{
    private string? createdBy;

    private DateTime? createDate;

    private DateTime? modifyDate;

    private string? modifiedBy;

    /// <summary>
    /// Дата создания доменного объекта
    /// </summary>
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        internal protected set => this.createDate = value;
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
        get => this.modifiedBy.TrimNull();
        protected internal set => this.modifiedBy = value.TrimNull();
    }

    /// <summary>
    /// Логин сотрудника, создавшего доменный объект
    /// </summary>
    public virtual string? CreatedBy
    {
        get => this.createdBy.TrimNull();
        internal protected set => this.createdBy = value.TrimNull();
    }
}
