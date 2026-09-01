using System.Data;

using Framework.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Database.EntityFramework.Sessions;

public class ReadOnlyEfSession : IEfSession
{
    private static readonly IDbTransaction DbTransaction = LazyInterfaceImplementHelper.CreateNotImplemented<IDbTransaction>("Readonly session");

    private readonly IDbContextTransaction transaction;

    public ReadOnlyEfSession(DbContext nativeSession)
    {
        this.NativeSession = nativeSession;
        this.NativeSession.Database.OpenConnection();

        //this.NativeSession.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        this.NativeSession.ChangeTracker.AutoDetectChangesEnabled = false;

        // need for support different isolation level (aka Snapshot)
        this.transaction = nativeSession.Database.BeginTransaction();
    }

    public DBSessionMode SessionMode { get; } = DBSessionMode.Read;

    public IDbTransaction Transaction { get; } = DbTransaction;

    public DbContext NativeSession { get; }

    public bool Closed { get; private set; }

    public void AsFault()
    {
    }

    public void AsReadOnly()
    {
    }

    public void AsWritable() => throw new InvalidOperationException("Readonly session already created");

    public async Task CloseAsync(CancellationToken ct)
    {
        if (this.Closed)
        {
            return;
        }

        this.Closed = true;

        await using (this.NativeSession)
        {
            await using (this.transaction)
            {
            }
        }
    }

    public async Task FlushAsync(CancellationToken ct) => throw new InvalidOperationException("Readonly session cannot be flushed");

    public async ValueTask DisposeAsync() => await this.CloseAsync(CancellationToken.None);
}
