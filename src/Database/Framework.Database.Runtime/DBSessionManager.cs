using Framework.Core;

namespace Framework.Database;

public class DBSessionManager(ILazyObject<IDBSession> lazyDbSession) : IDBSessionManager
{
    /// <summary>
    /// Tries to close existing DB session (if exists) and to flush events to DAL listeners
    /// </summary>
    public async Task TryCloseDbSessionAsync(CancellationToken cancellationToken)
    {
        if (lazyDbSession.IsValueCreated)
        {
            await lazyDbSession.Value.CloseAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Tries to mark existing DB session (if exists) as faulted to prevent further DB writes
    /// </summary>
    public void TryFaultDbSession()
    {
        if (lazyDbSession.IsValueCreated)
        {
            lazyDbSession.Value.AsFault();
        }
    }
}