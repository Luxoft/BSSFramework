using System.Globalization;

using CommonFramework;

namespace Framework.Tracking;

/// <summary>
/// Defines property changes
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct TrackingProperty<T>(string PropertyName, Maybe<T> PreviousValue, Maybe<T> CurrentValue)
{
    public string LowPropertyName { get; } = PropertyName.ToLower(CultureInfo.InvariantCulture);
}
