using System.Collections;
using System.Globalization;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;

namespace Framework.Tracking;

/// <summary>
/// Represents modified properties info
/// </summary>
public struct TrackingResult<TDomainObject> : IEnumerable<TrackingProperty>
{
    private readonly IReadOnlyCollection<TrackingProperty> properties;

    /// <summary>
    /// Creates new result instance
    /// </summary>
    /// <param name="modifiedProperties">Modified properties</param>
    public TrackingResult(IEnumerable<TrackingProperty>? modifiedProperties)
            : this() =>
        this.properties = (modifiedProperties ?? []).ToList();

    /// <summary>
    /// Get the trackingproperty by expression
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="propertyExpression">The property expression.</param>
    public TrackingProperty<TProperty> Property<TProperty>(Expression<Func<TDomainObject, TProperty>> propertyExpression)
    {
        var path = propertyExpression.ToPath().ToLower(CultureInfo.InvariantCulture);
        var property = this.properties.Single(z => z.LowPropertyName == path, () => new ArgumentException($"The {path} property was not modified"));

        return TrackingProperty.Create<TProperty>(property.PropertyName, property.PreviousValue, property.CurrentValue);
    }

    /// <summary>
    /// Gets modified properties info
    /// </summary>
    /// <param name="propertyExpression">Property get expression</param>
    /// <returns></returns>
    public TrackingProperty<IEnumerable<TProperty>> ManyProperty<TProperty>(Expression<Func<TDomainObject, IEnumerable<TProperty>>> propertyExpression)
    {
        var path = propertyExpression.ToPath().ToLower(CultureInfo.InvariantCulture);
        var property = this.properties.Single(z => z.LowPropertyName == path, () => new ArgumentException($"{path} property are not modified"));

        return TrackingProperty.Create<IEnumerable<TProperty>>(
                                                               property.PropertyName,
                                                               ((IEnumerable)property.PreviousValue ?? Enumerable.Empty<TProperty>()).Cast<TProperty>().ToList(),
                                                               ((IEnumerable)property.CurrentValue ?? Enumerable.Empty<TProperty>()).Cast<TProperty>().ToList());
    }

    /// <summary>
    /// Gets previous or default value
    /// </summary>
    /// <typeparam name="TProperty">Property return type</typeparam>
    /// <param name="propertyExpression">Property get expression</param>
    /// <param name="defaultValue">Default property value if no previous value found</param>
    /// <returns>Previous or default property value identified by expression specified</returns>
    public TProperty GetPrevValue<TProperty>(Expression<Func<TDomainObject, TProperty>> propertyExpression, TProperty defaultValue) => this.GetChange(propertyExpression).Match(z => z.PreviousValue.GetValueOrDefault(defaultValue), () => defaultValue);

    /// <inheritdoc />
    public IEnumerator<TrackingProperty> GetEnumerator() => this.properties.GetEnumerator();

    /// <summary>
    /// Shows whether the property specified by expression changed in the session
    /// </summary>
    /// <param name="propertyExpression">Property get expression</param>
    public bool HasChange<T>(Expression<Func<TDomainObject, T>> propertyExpression) =>
        this.properties.Select(z => z.LowPropertyName)
            .Contains(propertyExpression.ToPath(), StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    /// Shows whether properties specified by expression changed in the session
    /// </summary>
    /// <param name="propertyExpression">Property get expression</param>
    public bool HasChange(params Expression<Func<TDomainObject, object>>[] propertyExpression)
    {
        if (!propertyExpression.Any())
        {
            return this.properties.Any();
        }

        return this.properties.Select(z => z.LowPropertyName)
                   .Intersect(propertyExpression.Select(z => z.ToPath().ToLower(CultureInfo.InvariantCulture)))
                   .Any();
    }

    /// <summary>
    /// Gets changed property details. Property identified by expression specified
    /// </summary>
    /// <param name="propertyChanged">Property get expression</param>
    public Maybe<TrackingProperty<TProperty>> GetChange<TProperty>(Expression<Func<TDomainObject, TProperty>> propertyChanged)
    {
        var hasChange = this.HasChange(propertyChanged.Select(v => (object)v));

        if (hasChange)
        {
            return Maybe.Return(this.Property(propertyChanged));
        }
        else
        {
            return Maybe<TrackingProperty<TProperty>>.Nothing;
        }
    }

    /// <summary>
    /// Gets changed properties details. Properties identified by expression specified
    /// </summary>
    /// <param name="propertyChanged">Property get expression</param>
    public Maybe<TrackingProperty<IEnumerable<TProperty>>> GetChange<TProperty>(Expression<Func<TDomainObject, IEnumerable<TProperty>>> propertyChanged)
    {
        var hasChange = this.HasChange(propertyChanged.Select(v => (object)v));

        if (hasChange)
        {
            return Maybe.Return(this.ManyProperty(propertyChanged));
        }
        else
        {
            return Maybe<TrackingProperty<IEnumerable<TProperty>>>.Nothing;
        }
    }

    /// <summary>
    /// Gets whether only allowed properties changed in the session. Allowed changes defined by expression specified
    /// </summary>
    /// <param name="propertyExpression">Allowed properties get expression</param>
    public bool Only(params Expression<Func<TDomainObject, object>>[] propertyExpression) => !this.GetUnexpectedChangedProprties(propertyExpression).Any();

    /// <summary>
    /// Gets properties changes that changed unexpectedly in the session. Allowed changes defined by expression specified
    /// </summary>
    /// <param name="allowedPropertiesForChangingExpressions">Allowed properties get expression</param>
    public IEnumerable<TrackingProperty> GetUnexpectedChangedProprties(
            params Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)
    {
        var allowedPropertiesForChanging =
                allowedPropertiesForChangingExpressions.ToArray(expr => typeof(TDomainObject).GetProperty(expr.GetMemberName(), true));

        var mergeResult = this.GetMergeResult(allowedPropertiesForChanging, v => v.PropertyName, v => v.Name);

        return mergeResult.RemovingItems;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
