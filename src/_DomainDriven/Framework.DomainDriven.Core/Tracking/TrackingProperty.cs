using System.Globalization;

using Framework.Core;

namespace Framework.DomainDriven.BLL.Tracking;

public struct TrackingProperty
{
    private readonly string propertyName;

    private readonly string lowPropertyName;

    private readonly object previusValue;

    private readonly object currentValue;

    internal TrackingProperty(string propertyName, object previusValue, object currentValue)
            : this()
    {
        this.propertyName = propertyName;
        this.previusValue = previusValue;
        this.currentValue = currentValue;
        this.lowPropertyName = propertyName.ToLower(CultureInfo.InvariantCulture);
    }

    public string PropertyName => this.propertyName;

    public string LowPropertyName => this.lowPropertyName;

    public object PreviusValue => this.previusValue;

    public object CurrentValue => this.currentValue;

    public static TrackingProperty<T> Create<T>(string propertyName, object previusValue, object currentValue)
    {
        if (typeof(T).IsClass)
        {
            return new TrackingProperty<T>(propertyName, Maybe.Return((T)previusValue), Maybe.Return((T)currentValue));
        }

        return new TrackingProperty<T>(
                                       propertyName,
                                       previusValue == null ? Maybe<T>.Nothing : Maybe.Return((T)previusValue),
                                       currentValue == null ? Maybe<T>.Nothing : Maybe.Return<T>((T)currentValue));
    }

    public static ObsoleteTrackingProperty<T> CreateObsolete<T>(string propertyName, object previusValue, object currentValue)
    {
        return new ObsoleteTrackingProperty<T>(propertyName, (T)previusValue, (T)currentValue);
    }
}
