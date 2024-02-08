using System.Data;

using Framework.DomainDriven.DAL.Revisions;

using NHibernate;
using NHibernate.Envers.Patch;

namespace Framework.DomainDriven.NHibernate;

public class NHibSession : INHibSession
{
    private DBSessionMode? sessionMode;

    private readonly Lazy<INHibSession> lazyInnerSession;

    public NHibSession(NHibSessionEnvironment environment, INHibSessionSetup settings, IEnumerable<IDBSessionEventListener> eventListeners)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));

        this.lazyInnerSession = new Lazy<INHibSession>(() =>
        {
            switch (this.sessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyNHibSession(environment);

                case DBSessionMode.Write:
                    return new WriteNHibSession(environment, settings, eventListeners);

                default:
                    throw new InvalidOperationException();
            }
        });
    }

    public virtual IDBSession InnerSession => this.lazyInnerSession.Value;

    public IAuditReaderPatched AuditReader => this.lazyInnerSession.Value.AuditReader;

    public ISession NativeSession => this.lazyInnerSession.Value.NativeSession;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IDbTransaction Transaction => this.InnerSession.Transaction;

    public void RegisterModified<TDomainObject>(TDomainObject domainObject, ModificationType modificationType)
    {
        this.lazyInnerSession.Value.RegisterModified(domainObject, modificationType);
    }

    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        await this.InnerSession.FlushAsync(cancellationToken);
    }

    public long GetCurrentRevision() => this.InnerSession.GetCurrentRevision();

    public IEnumerable<ObjectModification> GetModifiedObjectsFromLogic() => this.InnerSession.GetModifiedObjectsFromLogic();

    public IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>() => this.InnerSession.GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>();

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
