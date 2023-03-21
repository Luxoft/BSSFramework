using System;

using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.Domain;

/// <summary>
///  Операция доменного типа
/// </summary>
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
    /// Конструктор
    /// </summary>
    /// <param name="domainType">Доменный тип</param>
    /// <param name="operation">Операция</param>
    public DomainTypeEventOperation(DomainType domainType, [NotNull] Enum operation)
            : this(domainType)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        this.Name = operation.ToString();
    }

    /// <summary>
    /// Доменный тип
    /// </summary>
    public virtual DomainType DomainType => this.domainType;

    DomainType IDetail<DomainType>.Master => this.DomainType;
}
