using System.Globalization;

namespace Framework.DomainDriven.BLL.Tracking;

/// <summary>
/// Obsolete structure. Don't use this
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete("Use TrackingProperty")]
public struct ObsoleteTrackingProperty<T>
{
    private readonly string propertyName;

    private readonly string lowPropertyName;

    private readonly T previusValue;

    private readonly T currentValue;

    /// <summary>
    /// Initializes new ObsoleteTrackingProperty instance
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="previusValue">Property's prev value</param>
    /// <param name="currentValue">Property's current value</param>
    internal ObsoleteTrackingProperty(string propertyName, T previusValue, T currentValue)
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
    /// Gets property name in low case
    /// </summary>
    public string LowPropertyName => this.lowPropertyName;

    /// <summary>
    /// Gets prev value
    /// </summary>
    public T PreviusValue => this.previusValue;

    /// <summary>
    /// Gets current value
    /// </summary>
    public T CurrentValue => this.currentValue;
}
