using System.ComponentModel;

using Framework.Application.Domain;
using Framework.BLL.Domain.Serialization;
using Framework.Database.Attributes;

// ReSharper disable once CheckNamespace
namespace SampleSystem.Domain;

/// <summary>
///     Базовый персистентный класс
/// </summary>
public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IVersionObject<long>
{
    private DateTime? createDate;

    private string? createdBy = "";

    private DateTime? modifyDate;

    private string? modifiedBy = "";

    private bool active = true;

    private long version;

    /// <summary>
    ///     Дата создания доменного объекта
    /// </summary>
    public virtual DateTime? CreateDate
    {
        get => this.createDate;
        protected internal set => this.createDate = value;
    }

    /// <summary>
    ///     Логин сотрудника, изменившего доменный объект
    /// </summary>
    public virtual string? CreatedBy
    {
        get => this.createdBy;
        protected internal set => this.createdBy = value;
    }

    /// <summary>
    ///     Логин сотрудника, создавшего доменный объект
    /// </summary>
    public virtual DateTime? ModifyDate
    {
        get => this.modifyDate;
        protected internal set => this.modifyDate = value;
    }

    /// <summary>
    ///     Дата изменения доменного объекта
    /// </summary>
    public virtual string? ModifiedBy
    {
        get => this.modifiedBy;
        protected internal set => this.modifiedBy = value;
    }

    /// <summary>
    ///     Признак активности доменного объекта
    /// </summary>
    [DefaultValue(true)]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual bool Active
    {
        get => this.active;
        set => this.active = value;
    }

    [Version]
    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration)]
    public virtual long Version
    {
        get => this.version;
        set => this.version = value;
    }
}
