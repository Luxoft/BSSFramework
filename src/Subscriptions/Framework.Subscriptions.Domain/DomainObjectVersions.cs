namespace Framework.Subscriptions.Domain;

public abstract record DomainObjectVersions
{
    public abstract Type DomainObjectType { get; }

    public abstract DomainObjectChangeType ChangeType { get; }

    public static DomainObjectVersions<TDomainObject> Create<TDomainObject>(TDomainObject? previous, TDomainObject? current)
        where TDomainObject : class => new(previous, current);
}

/// <summary>
/// Domain object version container.
/// </summary>
/// <typeparam name="TDomainObject">Domain object type</typeparam>
public record DomainObjectVersions<TDomainObject>(TDomainObject? Previous, TDomainObject? Current) : DomainObjectVersions
    where TDomainObject : class
{
    /// <summary>
    /// Domain object type
    /// </summary>
    /// <value>
    /// Domain object type.
    /// </value>
    public override Type DomainObjectType { get; } = typeof(TDomainObject);

    /// <summary>
    /// Returns the type of change that occurred to the domain object.
    /// </summary>
    /// <value>
    /// Type of change that occurred to the domain object.
    /// </value>
    public override DomainObjectChangeType ChangeType { get; } = GetChangeType(Previous, Current);

    /// <inheritdoc/>
    public override string ToString() => $"DomainObjectType: {this.DomainObjectType}, Previous: {this.Previous}, Current: {this.Current}";

    public async ValueTask<DomainObjectVersions<TNewDomainObject>> ChangeDomainObjectAsync<TNewDomainObject>(Func<TDomainObject, ValueTask<TNewDomainObject>> selector)
        where TNewDomainObject : class => new(this.Previous is null ? null : await selector(this.Previous), this.Current is null ? null : await selector(this.Current));

    private static DomainObjectChangeType GetChangeType(TDomainObject? previous, TDomainObject? current)
    {
        if (previous is null && current is not null)
        {
            return DomainObjectChangeType.Create;
        }
        else if (previous is not null && current is not null)
        {
            return DomainObjectChangeType.Update;
        }
        else if (previous is not null && current is null)
        {
            return DomainObjectChangeType.Delete;
        }
        else
        {
            throw new ArgumentException("Both arguments (previous and current) can't be null");
        }
    }
}
