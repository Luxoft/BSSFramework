using Framework.Core;

namespace Framework.Authorization.SecuritySystem;

internal static class TimeProviderExtensions
{
    public static bool IsActivePeriod(this TimeProvider timeProvider, IPeriodObject periodObject)
    {
        if (periodObject == null) throw new ArgumentNullException(nameof(periodObject));
        if (timeProvider == null) throw new ArgumentNullException(nameof(timeProvider));

        return periodObject.Period.Contains(timeProvider.GetToday());
    }
}
