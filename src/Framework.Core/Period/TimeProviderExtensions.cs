// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class TimeProviderExtensions
{
    public static DateTime GetToday(this TimeProvider timeProvider) => timeProvider.GetLocalNow().Date;

    public static Period GetCurrentMonth(this TimeProvider timeProvider) => timeProvider.GetLocalNow().DateTime.ToMonth();

    public static Period GetCurrentYear(this TimeProvider timeProvider) => timeProvider.GetLocalNow().DateTime.ToYear();

    public static Period GetNextMonth(this TimeProvider timeProvider) => timeProvider.GetLocalNow().DateTime.AddMonth().ToMonth();

    public static Period GetPrevMonth(this TimeProvider timeProvider) => timeProvider.GetLocalNow().DateTime.SubtractMonth().ToMonth();
}
