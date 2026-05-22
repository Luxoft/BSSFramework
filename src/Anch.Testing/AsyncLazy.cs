using System.Runtime.ExceptionServices;

using Anch.Threading;

namespace Anch.Testing;

public class AsyncLazy<T>(Func<CancellationToken, Task<T>> factory) : IAsyncDisposable
{
    private readonly IAsyncLocker asyncLocker = new AsyncLocker();

    private bool disposed;

    private T? value;

    private volatile bool initialized;

    private ExceptionDispatchInfo? exceptionDispatchInfo;

    public async Task<T> GetValueAsync(CancellationToken ct)
    {
        if (!this.initialized)
        {
            using (await this.asyncLocker.CreateScope(ct).ConfigureAwait(false))
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(nameof(AsyncLazy<T>));
                }
                else if (!this.initialized)
                {
                    try
                    {
                        this.value = await factory(ct).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        this.exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        this.initialized = true;
                    }
                }
            }
        }

        this.exceptionDispatchInfo?.Throw();

        return this.value!;
    }

    public bool Initialized => this.initialized;

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref this.disposed, true))
        {
            return;
        }

        using (this.asyncLocker)
        {
            using (await this.asyncLocker.CreateScope(CancellationToken.None).ConfigureAwait(false))
            {
                if (this.initialized && this.value is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else if (this.initialized && this.value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
