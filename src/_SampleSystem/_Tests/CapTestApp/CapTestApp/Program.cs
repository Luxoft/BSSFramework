using CapTestApp;

using DotNetCore.CAP;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Savorboard.CAP.InMemoryMessageQueue;

public class Program
{
    public static void Main(string[] args)
    {
        var cts = new System.Threading.CancellationTokenSource();
        var container = new ServiceCollection();

        container.AddLogging(x => x.AddConsole());
        container.AddCap(x =>
                         {
                             x.UseSqlServer(Constants.ConnectionString);
                             x.UseInMemoryMessageQueue();
                         });

        container.AddSingleton<EventSubscriber>();
        container.AddSingleton<EventPublisher>();

        var sp = container.BuildServiceProvider();

        sp.GetService<IBootstrapper>().BootstrapAsync(cts.Token);

        _ = Task.Run(async () => await sp.GetService<EventPublisher>().PublishAsync(cts), cts.Token);

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
                                               {
                                                   cts.Cancel();
                                               };

        Console.ReadLine();
    }
}
