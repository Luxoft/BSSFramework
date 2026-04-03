// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class TimeProviderExtensions
{
    extension(TimeProvider timeProvider)
    {
        public DateTime GetToday() => timeProvider.GetLocalNow().Date;

        public Period GetCurrentMonth() => timeProvider.GetLocalNow().DateTime.ToMonth();

        public Period GetCurrentYear() => timeProvider.GetLocalNow().DateTime.ToYear();

        public Period GetNextMonth() => timeProvider.GetLocalNow().DateTime.AddMonth().ToMonth();

        public Period GetPrevMonth() => timeProvider.GetLocalNow().DateTime.SubtractMonth().ToMonth();
    }
}
