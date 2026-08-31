using System.Data;

using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public class NHibSession : IDBSession<ISession>
{
    private DBSessionMode? customSessionMode;

    private readonly Lazy<IDBSession<ISession>> lazyInnerSession;

    public NHibSession(
        NHibSessionEnvironment environment,
        DBSessionSettings settings,
        IAuditPropertyFactory auditPropertyFactory,
        IAuditReaderPatched auditReader,
        IEnumerable<IDBSessionEventListener> eventListeners) =>
        this.lazyInnerSession = new Lazy<IDBSession<ISession>>(() =>
        {
            switch (this.customSessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyNHibSession(environment);

                case DBSessionMode.Write:
                    return new WriteNHibSession(environment, auditPropertyFactory, auditReader, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDBSession<ISession> InnerSession => this.lazyInnerSession.Value;

    public ISession NativeSession => this.InnerSession.NativeSession;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public Task FlushAsync(CancellationToken ct) => this.InnerSession.FlushAsync(ct);

    public void AsFault() => this.InnerSession.AsFault();

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
