using System;
using System.Collections.Generic;

namespace Framework.Configuration.Core;

/// <summary>
/// Контейнер версий доменного объекта.
/// </summary>
/// <typeparam name="T">Тип доменного объекта</typeparam>
public sealed class DomainObjectVersions<T> : IDomainObjectVersions
        where T : class
{
    /// <summary>
    /// Создаёт экземпляр класса <see cref="DomainObjectVersions{T}"/>.
    /// </summary>
    /// <param name="previous">Предыдущая версия доменного объекта.</param>
    /// <param name="current">Текущая версия доменного объекта.</param>
    public DomainObjectVersions(T previous, T current)
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
    public T Current { get; }

    /// <summary>
    /// Возвращает предыдущую версию доменного объекта.
    /// </summary>
    /// <value>
    /// Предыдущая версия доменного объекта.
    /// </value>
    public T Previous { get; }

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

    object IDomainObjectVersions.Previous => this.Previous;

    object IDomainObjectVersions.Current => this.Current;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"DomainObjectType: {this.DomainObjectType}, Previous: {this.Previous}, Current: {this.Current}";
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is DomainObjectVersions<T> && this.Equals((DomainObjectVersions<T>)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return (EqualityComparer<T>.Default.GetHashCode(this.Current) * 397) ^
                   EqualityComparer<T>.Default.GetHashCode(this.Previous);
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

        return previousType ?? currentType ?? typeof(T);
    }

    private bool Equals(DomainObjectVersions<T> other)
    {
        return EqualityComparer<T>.Default.Equals(this.Current, other.Current) &&
               EqualityComparer<T>.Default.Equals(this.Previous, other.Previous);
    }
}
