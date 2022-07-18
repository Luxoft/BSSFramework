using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.NHibernate;

public class NHibSession : IDBSession
{
    private DBSessionMode? sessionMode;

    private readonly Lazy<IDBSession> lazyInnerSession;

    public NHibSession([NotNull] NHibSessionEnvironment environment, INHibSessionSetup settings)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));

        this.lazyInnerSession = new Lazy<IDBSession>(() =>
        {
            switch (this.sessionMode ?? settings.DefaultSessionMode)
            {
                case DBSessionMode.Read:
                    return new ReadOnlyNHibSession(environment);

                case DBSessionMode.Write:
                    return new WriteNHibSession(environment, settings);

                default:
                    throw new InvalidOperationException();
            }
        });
    }

    public IDBSession InnerSession => this.lazyInnerSession.Value;

    public DBSessionMode SessionMode => this.InnerSession.SessionMode;

    public IObjectStateService GetObjectStateService()
    {
        return this.InnerSession.GetObjectStateService();
    }

    public IDALFactory<TPersistentDomainObjectBase, TIdent> GetDALFactory<TPersistentDomainObjectBase, TIdent>()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        return this.InnerSession.GetDALFactory<TPersistentDomainObjectBase, TIdent>();
    }

    public void Flush()
    {
        this.InnerSession.Flush();
    }

    public long GetCurrentRevision() => this.InnerSession.GetCurrentRevision();

    public IEnumerable<ObjectModification> GetModifiedObjectsFromLogic() => this.InnerSession.GetModifiedObjectsFromLogic();

    public IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>() => this.InnerSession.GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>();

    public void ManualFault() => this.InnerSession.ManualFault();

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

    public void Dispose()
    {
        if (this.lazyInnerSession.IsValueCreated)
        {
            this.InnerSession.Dispose();
        }
    }

    public event EventHandler<DALChangesEventArgs> Flushed
    {
        add => this.InnerSession.Flushed += value;
        remove => this.InnerSession.Flushed -= value;
    }

    public event EventHandler<DALChangesEventArgs> BeforeTransactionCompleted
    {
        add => this.InnerSession.BeforeTransactionCompleted += value;
        remove => this.InnerSession.BeforeTransactionCompleted -= value;
    }

    public event EventHandler<DALChangesEventArgs> AfterTransactionCompleted
    {
        add => this.InnerSession.AfterTransactionCompleted += value;
        remove => this.InnerSession.AfterTransactionCompleted -= value;
    }

    public event EventHandler Closed
    {
        add => this.InnerSession.Closed += value;
        remove => this.InnerSession.Closed -= value;
    }
}
