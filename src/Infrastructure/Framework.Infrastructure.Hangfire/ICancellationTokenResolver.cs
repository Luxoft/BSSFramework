namespace Framework.Infrastructure.Hangfire;

public interface ICancellationTokenResolver<in TArg>
{
    CancellationToken Resolve(TArg arg);
}
