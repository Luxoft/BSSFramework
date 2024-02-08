using DotNetCore.CAP;

namespace CapTestApp;

public class EventSubscriber : ICapSubscribe
{
    [CapSubscribe("sample.console.showtime")]
    public Task ShowTime(DateTime date)
    {
        Console.WriteLine(date);
        return Task.CompletedTask;
    }
}
