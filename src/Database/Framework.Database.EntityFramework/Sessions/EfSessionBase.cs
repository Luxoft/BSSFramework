using System.Data;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public abstract class EfSessionBase(DbContext nativeSession, DBSessionMode sessionMode) : IEfSession
{
    public DbContext NativeSession { get; } = nativeSession;

    public DBSessionMode SessionMode { get; } = sessionMode;

    public abstract bool Closed { get; }

    public abstract IDbTransaction Transaction { get; }

    public abstract Task FlushAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public long GetCurrentRevision()
    {
        throw new NotImplementedException("EF");
    }

    /// <inheritdoc />
    public long GetMaxRevision()
    {
        throw new NotImplementedException("EF");
    }

    public abstract void AsFault();

    /// <inheritdoc />
    public abstract void AsReadOnly();

    /// <inheritdoc />
    public abstract void AsWritable();

    public abstract Task CloseAsync(CancellationToken cancellationToken = default);

    public async ValueTask DisposeAsync()
    {
        await this.CloseAsync();
    }
}
