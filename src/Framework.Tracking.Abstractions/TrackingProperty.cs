using System.Globalization;

using CommonFramework.Maybe;

namespace Framework.DomainDriven.Tracking;

public struct TrackingProperty
{
    private readonly string propertyName;

    private readonly string lowPropertyName;

    private readonly object previousValue;

    private readonly object currentValue;

    internal TrackingProperty(string propertyName, object previousValue, object currentValue)
            : this()
    {
        this.propertyName = propertyName;
        this.previousValue = previousValue;
        this.currentValue = currentValue;
        this.lowPropertyName = propertyName.ToLower(CultureInfo.InvariantCulture);
    }

    public string PropertyName => this.propertyName;

    public string LowPropertyName => this.lowPropertyName;

    public object PreviousValue => this.previousValue;

    public object CurrentValue => this.currentValue;

    public static TrackingProperty<T> Create<T>(string propertyName, object previousValue, object currentValue)
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
