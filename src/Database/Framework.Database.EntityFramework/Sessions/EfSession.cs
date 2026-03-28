using System.Data;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public class EfSession : IEfSession
{
    private DBSessionMode? sessionMode;

    private readonly Lazy<IEfSession> lazyInnerSession;

    public EfSession(DbContext nativeSession, IDBSessionSettings settings, IEnumerable<IDBSessionEventListener> eventListeners)
    {
        this.lazyInnerSession = new Lazy<IEfSession>(() =>
        {
            switch (this.sessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyEfSession(nativeSession);

                case DBSessionMode.Write:
                    return new WriteEfSession(nativeSession, settings, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });
    }

    public virtual IDBSession InnerSession => this.lazyInnerSession.Value;

    public DbContext NativeSession => this.lazyInnerSession.Value.NativeSession;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        await this.InnerSession.FlushAsync(cancellationToken);
    }

    public long GetCurrentRevision() => this.InnerSession.GetCurrentRevision();

    public void AsFault() => this.InnerSession.AsFault();

    public long GetMaxRevision() => this.InnerSession.GetMaxRevision();

    public void AsReadOnly()
    {
        this.ApplySessionMode(DBSessionMode.Read);
    }

    public void AsWritable()
    {
        this.ApplySessionMode(DBSessionMode.Write);
    }

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

    public async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (this.lazyInnerSession.IsValueCreated)
        {
            await this.InnerSession.CloseAsync(cancellationToken);
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
