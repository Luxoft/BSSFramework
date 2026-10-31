using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Target system domain type description
/// </summary>
[NotAuditedClass]
public class DomainType : BaseDirectory, IDetail<TargetSystem>, IMaster<DomainTypeEventOperation>
{
    private readonly ICollection<DomainTypeEventOperation> eventOperations = new List<DomainTypeEventOperation>();

    private readonly TargetSystem targetSystem = null!;

    private string @namespace = "";

    protected DomainType()
    {
    }

    /// <summary>
    /// Target system domain type constructor
    /// </summary>
    /// <param name="targetSystem">Target system</param>
    public DomainType(TargetSystem targetSystem)
    {
        this.targetSystem = targetSystem ?? throw new ArgumentNullException(nameof(targetSystem));
        this.targetSystem.AddDetail(this);
    }

    /// <summary>
    /// Target system
    /// </summary>
    public virtual TargetSystem TargetSystem => this.targetSystem;

    /// <summary>
    /// Domain type operations
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<DomainTypeEventOperation> EventOperations => this.eventOperations;

    /// <summary>
    /// Namespace
    /// </summary>
    [UniqueElement]
    public virtual string Namespace { get => this.@namespace; set => this.@namespace = value; }

    /// <summary>
    /// Full type name
    /// </summary>
    public virtual string FullTypeName =>
        string.IsNullOrEmpty(this.Namespace)
            ? this.Name
            : $"{this.Namespace}.{this.Name}";

    public static implicit operator TypeNameIdentity(DomainType domainType) => new() { Namespace = domainType.Namespace, Name = domainType.Name };

    #region IDetail<TargetSystem> Members

    TargetSystem IDetail<TargetSystem>.Master => this.TargetSystem;

    #endregion

    ICollection<DomainTypeEventOperation> IMaster<DomainTypeEventOperation>.Details => (ICollection<DomainTypeEventOperation>)this.EventOperations;
}
