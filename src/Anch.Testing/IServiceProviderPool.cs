using Anch.Threading;

namespace Anch.Testing;

public interface IServiceProviderPool : IAsyncDisposable
{
    IAsyncLocker AsyncLocker { get; }

    int GlobalMainIndex { get; }

    int MainIndex { get; }

    bool IsRoot { get; }

    object TestFramework { get; }

    IServiceProviderPool? Inner { get; }

    ValueTask<IServiceProvider> GetAsync(CancellationToken ct);

    ValueTask ReleaseAsync(IServiceProvider serviceProvider, CancellationToken ct);
}
