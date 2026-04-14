using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

/// <summary>
/// Описание доменного типа целевой системы
/// </summary>
[NotAuditedClass]
public class DomainType : BaseDirectory, IDetail<TargetSystem>, IMaster<DomainTypeEventOperation>
{
    private readonly ICollection<DomainTypeEventOperation> eventOperations = new List<DomainTypeEventOperation>();

    private readonly TargetSystem targetSystem;

    private string @namespace;

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
    public virtual TargetSystem TargetSystem => this.targetSystem;

    /// <summary>
    /// Операции доменного типа
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<DomainTypeEventOperation> EventOperations => this.eventOperations;

    /// <summary>
    /// Пространство имен
    /// </summary>
    [UniqueElement]
    public virtual string Namespace { get => this.@namespace.TrimNull(); set => this.@namespace = value.TrimNull(); }

    /// <summary>
    /// Полное имя типа
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
