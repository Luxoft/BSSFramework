using Hangfire.Server;

namespace Framework.Infrastructure.Hangfire;

public class PerformContextCancellationTokenResolver : ICancellationTokenResolver<PerformContext>
{
    public CancellationToken Resolve(PerformContext arg) => arg.CancellationToken.ShutdownToken;
}
