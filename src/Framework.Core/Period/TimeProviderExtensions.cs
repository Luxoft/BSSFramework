// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class TimeProviderExtensions
{
    extension(TimeProvider timeProvider)
    {
        public DateTime GetToday() => DateTime.SpecifyKind(timeProvider.GetLocalNow().Date, DateTimeKind.Local);

        public Period GetCurrentMonth() => DateTime.SpecifyKind(timeProvider.GetLocalNow().DateTime, DateTimeKind.Local).ToMonth();

        public Period GetCurrentYear() => DateTime.SpecifyKind(timeProvider.GetLocalNow().DateTime, DateTimeKind.Local).ToYear();

        public Period GetNextMonth() => DateTime.SpecifyKind(timeProvider.GetLocalNow().DateTime, DateTimeKind.Local).AddMonth().ToMonth();

        public Period GetPrevMonth() => DateTime.SpecifyKind(timeProvider.GetLocalNow().DateTime, DateTimeKind.Local).SubtractMonth().ToMonth();
    }
}
