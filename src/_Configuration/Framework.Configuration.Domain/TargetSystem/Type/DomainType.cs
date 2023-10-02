using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Security;

namespace Framework.Configuration.Domain;

/// <summary>
/// Описание доменного типа целевой системы
/// </summary>
[BLLViewRole]
[NotAuditedClass]
public class DomainType : BaseDirectory, ITargetSystemElement<TargetSystem>, IDetail<TargetSystem>, IMaster<DomainTypeEventOperation>, IDomainType
{
    private readonly ICollection<DomainTypeEventOperation> eventOperations = new List<DomainTypeEventOperation>();

    private readonly TargetSystem targetSystem;

    private string nameSpace;

    protected DomainType()
    {
    }

    /// <summary>
    /// Конструктор доменного типа целовой системы
    /// </summary>
    /// <param name="targetSystem">Целевая система</param>
    public DomainType(TargetSystem targetSystem)
    {
        if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

        this.targetSystem = targetSystem;
        this.targetSystem.AddDetail(this);
    }

    /// <summary>
    /// Целевая система
    /// </summary>
    public virtual TargetSystem TargetSystem
    {
        get { return this.targetSystem; }
    }

    /// <summary>
    /// Операции доменного типа
    /// </summary>
    [UniqueGroup]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual IEnumerable<DomainTypeEventOperation> EventOperations => this.eventOperations;

    /// <summary>
    /// Название доменного типа
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }

    /// <summary>
    /// Пространство имен
    /// </summary>
    [UniqueElement]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual string NameSpace
    {
        get { return this.nameSpace.TrimNull(); }
        set { this.nameSpace = value.TrimNull(); }
    }

    /// <summary>
    /// Полное имя типа
    /// </summary>
    public virtual string FullTypeName
    {
        get
        {
            return string.IsNullOrEmpty(this.NameSpace)
                           ? this.Name
                           : $"{this.NameSpace}.{this.Name}";
        }
    }

    #region IDetail<TargetSystem> Members

    TargetSystem IDetail<TargetSystem>.Master
    {
        get { return this.TargetSystem; }
    }

    #endregion

    ICollection<DomainTypeEventOperation> IMaster<DomainTypeEventOperation>.Details
    {
        get { return (ICollection<DomainTypeEventOperation>)this.EventOperations; }
    }
}
