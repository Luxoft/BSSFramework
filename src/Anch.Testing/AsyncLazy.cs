using System.Runtime.ExceptionServices;

using Anch.Threading;

namespace Anch.Testing;

public class AsyncLazy<T>(Func<CancellationToken, Task<T>> factory)
{
    private readonly IAsyncLocker asyncLocker = new AsyncLocker();

    private T? value;

    private volatile bool initialized;

    private ExceptionDispatchInfo? exceptionDispatchInfo;

    public async Task<T> GetValueAsync(CancellationToken ct)
    {
        if (!this.initialized)
        {
            using (await this.asyncLocker.CreateScope(ct).ConfigureAwait(false))
            {
                if (!this.initialized)
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
}
