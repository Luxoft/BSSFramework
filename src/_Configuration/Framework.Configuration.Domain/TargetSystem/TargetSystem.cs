using System.Collections.Generic;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Целевая сиcтема
/// </summary>
/// <remarks>
/// Целевая сиcтема может содержать подсистемы
/// </remarks>
[BLLViewRole, BLLSaveRole(AllowCreate = false)]
[UniqueGroup]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.TargetSystemView)]
[ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.TargetSystemEdit)]
[NotAuditedClass]
public class TargetSystem : BaseDirectory, IMaster<DomainType>
{
    private readonly ICollection<DomainType> domainTypes = new List<DomainType>();

    private readonly bool isBase;

    private readonly bool isMain;

    private readonly bool isRevision;

    private bool subscriptionEnabled;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="isBase">Признак целевой системы типов, содержащихся в системных библиотеках</param>
    /// <param name="isMain">Признак главной целевой системы</param>
    /// <param name="isRevision">Признак поддержки системой ревизии баз данных</param>
    public TargetSystem(bool isBase, bool isMain, bool isRevision)
    {
        this.isBase = isBase;
        this.isMain = isMain;
        this.isRevision = isRevision;
    }

    protected TargetSystem()
    {
    }

    /// <summary>
    /// Коллекция доменных типов целевой системы
    /// </summary>
    [UniqueGroup]
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual IEnumerable<DomainType> DomainTypes
    {
        get { return this.domainTypes; }
    }

    /// <summary>
    /// Признак целевой системы, которая создана для типов, содержащихся в системных библиотеках
    /// </summary>
    public virtual bool IsBase
    {
        get { return this.isBase; }
    }

    /// <summary>
    /// Признак главной целевой системы
    /// </summary>
    public virtual bool IsMain
    {
        get { return this.isMain; }
    }

    /// <summary>
    /// Признак поддержки целевой системой ревизизи баз данных
    /// </summary>
    public virtual bool IsRevision
    {
        get { return this.isRevision; }
    }

    /// <summary>
    /// Признак того, что включен механизм подписки
    /// </summary>
    public virtual bool SubscriptionEnabled
    {
        get { return this.subscriptionEnabled; }
        set { this.subscriptionEnabled = value; }
    }

    #region IMaster<DomainType> Members

    ICollection<DomainType> IMaster<DomainType>.Details
    {
        get { return (ICollection<DomainType>)this.DomainTypes; }
    }

    #endregion
}
