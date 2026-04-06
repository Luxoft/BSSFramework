namespace Framework.Subscriptions.Domain;

public abstract record DomainObjectVersions
{
    public abstract Type DomainObjectType { get; }

    public abstract DomainObjectChangeType ChangeType { get; }
}

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
/// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
public record DomainObjectVersions<TDomainObject>(TDomainObject? Previous, TDomainObject? Current) : DomainObjectVersions
    where TDomainObject : class
{
    /// <summary>
    /// Тип доменного объекта
    /// </summary>
    /// <value>
    /// Тип доменного объекта.
    /// </value>
    public override Type DomainObjectType { get; } = typeof(TDomainObject);

    /// <summary>
    /// Возвращает тип изменения, произошедшего с доменным объектом.
    /// </summary>
    /// <value>
    /// Тип изменения, произошедшего с доменным объектом.
    /// </value>
    public override DomainObjectChangeType ChangeType { get; } = GetChangeType(Previous, Current);

    /// <inheritdoc/>
    public override string ToString() => $"DomainObjectType: {this.DomainObjectType}, Previous: {this.Previous}, Current: {this.Current}";

    public DomainObjectVersions<TNewDomainObject> ChangeDomainObject<TNewDomainObject>(Func<TDomainObject, TNewDomainObject> selector)
        where TNewDomainObject : class => new(this.Previous == null ? null : selector(this.Previous), this.Current == null ? null : selector(this.Current));

    private static DomainObjectChangeType GetChangeType(TDomainObject? previous, TDomainObject? current)
    {
        if (previous == null && current != null)
        {
            return DomainObjectChangeType.Create;
        }
        else if (previous != null && current != null)
        {
            return DomainObjectChangeType.Update;
        }
        else if (previous != null && current == null)
        {
            return DomainObjectChangeType.Delete;
        }
        else
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }
    }
}
