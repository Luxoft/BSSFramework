using System.Data;

using Framework.Core;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.Sessions;

public class EfSession<TDbContext> : IDBSession<TDbContext>
    where TDbContext : DbContext
{
    private DBSessionMode? sessionMode;

    public EfSession(TDbContext nativeSession, DBSessionSettings settings, IEnumerable<IDBSessionEventListener> eventListeners) =>
        this.LazyInnerSession = new LazyObject<IDBSession<TDbContext>>(() =>
        {
            switch (this.sessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyEfSession<TDbContext>(nativeSession);

                case DBSessionMode.Write:
                    return new WriteEfSession<TDbContext>(nativeSession, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });

    public LazyObject<IDBSession<TDbContext>> LazyInnerSession { get; }

    public IDBSession<TDbContext> InnerSession => this.LazyInnerSession.Value;

    public TDbContext NativeSession => this.InnerSession.NativeSession;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public async Task FlushAsync(CancellationToken ct) => await this.InnerSession.FlushAsync(ct);

    public void AsFault() => this.InnerSession.AsFault();

    public void AsReadOnly() => this.ApplySessionMode(DBSessionMode.Read);

    public void AsWritable() => this.ApplySessionMode(DBSessionMode.Write);

    private void ApplySessionMode(DBSessionMode applySessionMode)
    {
        if (!this.LazyInnerSession.IsValueCreated)
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
        if (this.LazyInnerSession.IsValueCreated)
        {
            await this.InnerSession.CloseAsync(ct);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (this.LazyInnerSession.IsValueCreated)
        {
            await this.InnerSession.DisposeAsync();
        }
    }
}
