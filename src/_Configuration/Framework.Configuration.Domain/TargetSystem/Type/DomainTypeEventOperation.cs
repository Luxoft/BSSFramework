using Framework.Database.Mapping;
using Framework.Relations;

namespace Framework.Configuration.Domain;

/// <summary>
///  Операция доменного типа
/// </summary>
[NotAuditedClass]
public class DomainTypeEventOperation : BaseDirectory, IDetail<DomainType>
{
    private readonly DomainType domainType;

    protected DomainTypeEventOperation()
    {
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="domainType">Доменный тип</param>
    public DomainTypeEventOperation(DomainType domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        this.domainType = domainType;
        this.domainType.AddDetail(this);
    }

    /// <summary>
    /// Доменный тип
    /// </summary>
    public virtual DomainType DomainType => this.domainType;

    DomainType IDetail<DomainType>.Master => this.DomainType;
}
