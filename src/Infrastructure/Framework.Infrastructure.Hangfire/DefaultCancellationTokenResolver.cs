using Anch.Core;

namespace Framework.Infrastructure.Hangfire;

public class DefaultCancellationTokenResolver<TArg>(IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : ICancellationTokenResolver<TArg>
{
    public CancellationToken Resolve(TArg arg) => defaultCancellationTokenSource?.CancellationToken ?? CancellationToken.None;
}
