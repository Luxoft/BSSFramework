using System.Data;

namespace Framework.Database;

public interface IDBSession : IAsyncDisposable, IDisposable
{
    DBSessionMode SessionMode { get; }

    IDbTransaction Transaction { get; }

    Task FlushAsync(CancellationToken ct);

    /// <summary>
    /// Closes the current transaction without applying changes.
    /// </summary>
    void AsFault();

    /// <summary>
    /// Switches the session to read-only mode (available only before the session is actually used)
    /// </summary>
    void AsReadOnly();

    /// <summary>
    /// Switches the session to write mode (available only before the session is actually used)
    /// </summary>
    void AsWritable();

    Task CloseAsync(CancellationToken ct);

    /// <summary>
    /// Synchronous dispose blocks on <see cref="IAsyncDisposable.DisposeAsync"/>, which commits the transaction (see <see cref="CloseAsync"/>).
    /// Prefer <c>await using</c>: blocking here can deadlock under a SynchronizationContext.
    /// </summary>
    void IDisposable.Dispose() => this.DisposeAsync().GetAwaiter().GetResult();
}

public interface IDBSession<out TNativeSession> : IDBSession
{
    TNativeSession NativeSession { get; }
}
