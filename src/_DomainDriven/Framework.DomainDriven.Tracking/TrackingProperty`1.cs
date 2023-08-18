using System.Globalization;

using Framework.Core;

namespace Framework.DomainDriven.Tracking;

/// <summary>
/// Defines property changes
/// </summary>
/// <typeparam name="T"></typeparam>
public struct TrackingProperty<T>
{
    private readonly string propertyName;

    private readonly string lowPropertyName;

    private readonly Maybe<T> previusValue;

    private readonly Maybe<T> currentValue;

    /// <summary>
    /// Initializes new TrackingProperty instance
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="previusValue">Property's prev value</param>
    /// <param name="currentValue">Property's current value</param>
    internal TrackingProperty(string propertyName, Maybe<T> previusValue, Maybe<T> currentValue)
            : this()
    {
        this.propertyName = propertyName;
        this.previusValue = previusValue;
        this.currentValue = currentValue;
        this.lowPropertyName = propertyName.ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets property name
    /// </summary>
    public string PropertyName => this.propertyName;

    /// <summary>
    /// Gets property name in lower case
    /// </summary>
    public string LowPropertyName => this.lowPropertyName;

    /// <summary>
    /// Gets property's previous value
    /// </summary>
    public Maybe<T> PreviusValue => this.previusValue;

    /// <summary>
    /// Gets property's current value
    /// </summary>
    public Maybe<T> CurrentValue => this.currentValue;
}
