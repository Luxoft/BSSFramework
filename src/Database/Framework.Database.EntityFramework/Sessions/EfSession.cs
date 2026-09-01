using System.Data;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.Sessions;

public class EfSession<TDbContext> : IEfSession
    where TDbContext : DbContext
{
    private DBSessionMode? sessionMode;

    private readonly Lazy<IEfSession> lazyInnerSession;

    public EfSession(TDbContext nativeSession, DBSessionSettings settings, IEnumerable<IDBSessionEventListener> eventListeners) =>
        this.lazyInnerSession = new Lazy<IEfSession>(() =>
        {
            switch (this.sessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyEfSession(nativeSession);

                case DBSessionMode.Write:
                    return new WriteEfSession(nativeSession, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });

    public IDBSession InnerSession => this.lazyInnerSession.Value;

    public DbContext NativeSession => this.lazyInnerSession.Value.NativeSession;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public async Task FlushAsync(CancellationToken ct) => await this.InnerSession.FlushAsync(ct);

    public void AsFault() => this.InnerSession.AsFault();

    public void AsReadOnly() => this.ApplySessionMode(DBSessionMode.Read);

    public void AsWritable() => this.ApplySessionMode(DBSessionMode.Write);

    private void ApplySessionMode(DBSessionMode applySessionMode)
    {
        if (!this.lazyInnerSession.IsValueCreated)
        {
            this.sessionMode = applySessionMode;
        }
        else if (this.SessionMode != applySessionMode)
        {
            throw new InvalidOperationException("Session mode can't be changed after create session");
        }
    }

    public async Task CloseAsync(CancellationToken ct)
    {
        if (this.lazyInnerSession.IsValueCreated)
        {
            await this.InnerSession.CloseAsync(ct);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (this.lazyInnerSession.IsValueCreated)
        {
            await this.InnerSession.DisposeAsync();
        }
    }
}
