using Framework.Database.Mapping;
using Framework.Relations;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
///  Domain type operation
/// </summary>
[NotAuditedClass]
public class DomainTypeEventOperation : BaseDirectory, IDetail<DomainType>
{
    private readonly DomainType domainType = null!;

    protected DomainTypeEventOperation()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="domainType">Domain type</param>
    public DomainTypeEventOperation(DomainType domainType)
    {
        this.domainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.domainType.AddDetail(this);
    }

    /// <summary>
    /// Domain type
    /// </summary>
    public virtual DomainType DomainType => this.domainType;

    DomainType IDetail<DomainType>.Master => this.DomainType;
}
