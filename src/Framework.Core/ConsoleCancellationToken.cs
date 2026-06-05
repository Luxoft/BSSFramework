using System.Runtime.InteropServices;

namespace Framework.Core;

public static class ConsoleCancellationToken
{
    public static async Task Run(Func<CancellationToken, Task> mainTask, int delayMilliseconds = 4000)
    {
        using var cts = new CancellationTokenSource();
        var canceling = 0;

        void Cancel()
        {
            if (Interlocked.Exchange(ref canceling, 1) != 0)
                return;
            cts.Cancel();
        }

        var cancelKeyPress = (ConsoleCancelEventHandler)((_, e) => { e.Cancel = true; Cancel(); });
        var processExit = (EventHandler)((_, _) => Cancel());

        Console.CancelKeyPress += cancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += processExit;
        using var sigterm = PosixSignalRegistration.Create(PosixSignal.SIGTERM, _ => Cancel());

        try
        {
            var task = mainTask(cts.Token);

            var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));

            if (completedTask == task)
            {
                await task;
                return;
            }

            if (!task.IsCompleted)
            {
                using var delayCts = new CancellationTokenSource();
                var delayTask = Task.Delay(delayMilliseconds, delayCts.Token);

                await Task.WhenAny(task, delayTask);

                await delayCts.CancelAsync();
            }

            if (task.IsCompleted)
                await task;
        }
        finally
        {
            Console.CancelKeyPress -= cancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit -= processExit;
        }
    }
}

