namespace Framework.Infrastructure.Hangfire;

public class ExplicitCancellationTokenResolver : ICancellationTokenResolver<CancellationToken>
{
    public CancellationToken Resolve(CancellationToken arg) => arg;
}
