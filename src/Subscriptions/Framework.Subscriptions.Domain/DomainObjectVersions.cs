namespace Framework.Subscriptions.Domain;

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
/// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
public sealed class DomainObjectVersions<TDomainObject> : IDomainObjectVersions
    where TDomainObject : class
{
    /// <summary>
    /// Создаёт экземпляр класса <see cref="DomainObjectVersions{T}"/>.
    /// </summary>
    /// <param name="previous">Предыдущая версия доменного объекта.</param>
    /// <param name="current">Текущая версия доменного объекта.</param>
    public DomainObjectVersions(TDomainObject? previous, TDomainObject? current)
    {
        if (previous == null && current == null)
        {
            throw new ArgumentException("both arguments (previous and current) can't be null");
        }

        this.Current = current;
        this.Previous = previous;
        this.DomainObjectType = this.GetDomainObjectType();
    }

    /// <summary>
    /// Возвращает текущую версию доменного объекта.
    /// </summary>
    /// <value>
    /// Текущая версия доменного объекта.
    /// </value>
    public TDomainObject? Current { get; }

    /// <summary>
    /// Возвращает предыдущую версию доменного объекта.
    /// </summary>
    /// <value>
    /// Предыдущая версия доменного объекта.
    /// </value>
    public TDomainObject? Previous { get; }

    /// <summary>
    /// Реальный тип версий доменного объекта, сохранённый в этом экземпляре.
    /// </summary>
    /// <value>
    /// Тип доменного объекта.
    /// </value>
    public Type DomainObjectType { get; }

    /// <summary>
    /// Возвращает тип изменения, произошедшего с доменным объектом.
    /// </summary>
    /// <value>
    /// Тип изменения, произошедшего с доменным объектом.
    /// </value>
    public DomainObjectChangeType ChangeType
    {
        get
        {
            if (this.Previous == null && this.Current != null)
            {
                return DomainObjectChangeType.Create;
            }

            if (this.Previous != null && this.Current != null)
            {
                return DomainObjectChangeType.Update;
            }

            if (this.Previous != null && this.Current == null)
            {
                return DomainObjectChangeType.Delete;
            }

            return DomainObjectChangeType.Unknown;
        }
    }

    object? IDomainObjectVersions.Previous => this.Previous;

    object? IDomainObjectVersions.Current => this.Current;

    /// <inheritdoc/>
    public override string ToString() => $"DomainObjectType: {this.DomainObjectType}, Previous: {this.Previous}, Current: {this.Current}";

    public DomainObjectVersions<TNewDomainObject> ChangeDomainObject<TNewDomainObject>(Func<TDomainObject, TNewDomainObject> selector)
        where TNewDomainObject : class => new(this.Previous == null ? null : selector(this.Previous), this.Current == null ? null : selector(this.Current));

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is DomainObjectVersions<TDomainObject> && this.Equals((DomainObjectVersions<TDomainObject>)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return (EqualityComparer<TDomainObject>.Default.GetHashCode(this.Current) * 397) ^ EqualityComparer<TDomainObject>.Default.GetHashCode(this.Previous);
        }
    }

    private Type GetDomainObjectType()
    {
        var previousType = this.Previous?.GetType();
        var currentType = this.Current?.GetType();

        if (previousType != null && currentType != null && previousType != currentType)
        {
            return typeof(object);
        }

        return previousType ?? currentType ?? typeof(TDomainObject);
    }

    private bool Equals(DomainObjectVersions<TDomainObject> other) => EqualityComparer<TDomainObject>.Default.Equals(this.Current, other.Current) && EqualityComparer<TDomainObject>.Default.Equals(this.Previous, other.Previous);
}
