using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Target system
/// </summary>
/// <remarks>
/// A target system can contain subsystems
/// </remarks>
[UniqueGroup]
[NotAuditedClass]
public class TargetSystem : BaseDirectory, IMaster<DomainType>
{
    private readonly ICollection<DomainType> domainTypes = new List<DomainType>();

    private readonly bool isBase;

    private readonly bool isMain;

    private readonly bool isRevision;

    private bool subscriptionEnabled;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="isBase">Flag of the target system for types contained in system libraries</param>
    /// <param name="isMain">Flag of the main target system</param>
    /// <param name="isRevision">Flag of the system's database revision support</param>
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
    /// Collection of target system domain types
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<DomainType> DomainTypes => this.domainTypes;

    /// <summary>
    /// Flag of a target system created for types contained in system libraries
    /// </summary>
    public virtual bool IsBase => this.isBase;

    /// <summary>
    /// Flag of the main target system
    /// </summary>
    public virtual bool IsMain => this.isMain;

    /// <summary>
    /// Flag of the target system's database revision support
    /// </summary>
    public virtual bool IsRevision => this.isRevision;

    /// <summary>
    /// Flag indicating that the subscription mechanism is enabled
    /// </summary>
    public virtual bool SubscriptionEnabled
    {
        get => this.subscriptionEnabled;
        set => this.subscriptionEnabled = value;
    }

    #region IMaster<DomainType> Members

    ICollection<DomainType> IMaster<DomainType>.Details => (ICollection<DomainType>)this.DomainTypes;

    #endregion
}
