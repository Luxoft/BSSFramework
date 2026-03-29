using System.Data;

using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public class NHibSession : INHibSession
{
    private DBSessionMode? customSessionMode;

    private readonly Lazy<INHibSession> lazyInnerSession;

    public NHibSession(
        NHibSessionEnvironment environment,
        DBSessionSettings settings,
        IAuditPropertyFactory auditPropertyFactory,
        IEnumerable<IDBSessionEventListener> eventListeners) =>
        this.lazyInnerSession = new Lazy<INHibSession>(() =>
        {
            switch (this.customSessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyNHibSession(environment);

                case DBSessionMode.Write:
                    return new WriteNHibSession(environment, auditPropertyFactory, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });

    public IDBSession InnerSession => this.lazyInnerSession.Value;

    public IAuditReaderPatched AuditReader => this.lazyInnerSession.Value.AuditReader;

    public ISession NativeSession => this.lazyInnerSession.Value.NativeSession;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public Task FlushAsync(CancellationToken cancellationToken) => this.InnerSession.FlushAsync(cancellationToken);

    public long GetCurrentRevision() => this.InnerSession.GetCurrentRevision();

    public void AsFault() => this.InnerSession.AsFault();

    public long GetMaxRevision() => this.InnerSession.GetMaxRevision();

    public void AsReadOnly() => this.ApplySessionMode(DBSessionMode.Read);

    public void AsWritable() => this.ApplySessionMode(DBSessionMode.Write);

    private void ApplySessionMode(DBSessionMode applySessionMode)
    {
        if (!this.lazyInnerSession.IsValueCreated)
        {
            this.customSessionMode = applySessionMode;
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
