using System.Globalization;

using Anch.Core;

namespace Framework.Tracking;

public readonly record struct TrackingProperty(string PropertyName, object? PreviousValue, object? CurrentValue)
{
    public string LowPropertyName { get; } = PropertyName.ToLower(CultureInfo.InvariantCulture);

    public static TrackingProperty<T> Create<T>(string propertyName, object? previousValue, object? currentValue)
    {
        if (typeof(T).IsClass)
        {
            return new TrackingProperty<T>(propertyName, Maybe.Return((T)previousValue), Maybe.Return((T)currentValue));
        }
        else
        {
            return new TrackingProperty<T>(
                propertyName,
                previousValue == null ? Maybe<T>.Nothing : Maybe.Return((T)previousValue),
                currentValue == null ? Maybe<T>.Nothing : Maybe.Return<T>((T)currentValue));
        }
    }
}
