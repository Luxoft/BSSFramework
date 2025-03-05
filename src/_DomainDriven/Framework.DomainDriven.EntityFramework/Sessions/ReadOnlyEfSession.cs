using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.DomainDriven.EntityFramework;

public class ReadOnlyEfSession : EfSessionBase
{
    private bool closed;
    private readonly IDbContextTransaction transaction;

    public ReadOnlyEfSession(DbContext nativeSession)
            : base(nativeSession, DBSessionMode.Read)
    {
        nativeSession.Database.OpenConnection();

        nativeSession.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        nativeSession.ChangeTracker.AutoDetectChangesEnabled = false;

        // need for support different isolation level (aka Snapshot)
        this.transaction = this.NativeSession.Database.BeginTransaction();
    }

    public override bool Closed => this.closed;

    public override void AsFault()
    {
    }

    public override void AsReadOnly()
    {
    }

    public override void AsWritable()
    {
        throw new InvalidOperationException("Readonly session already created");
    }

    public override async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (this.closed)
        {
            return;
        }

        this.closed = true;

        await using (this.NativeSession)
        {
            await using (this.transaction)
            {
            }
        }
    }

    public override IDbTransaction Transaction { get; } = null;

    public override async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }
}
