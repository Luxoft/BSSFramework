using NHibernate;
using NHibernate.Engine;
using NHibernate.Envers.Event;

namespace Framework.Database.NHibernate.Envers;

public interface IAuditReaderPatchedFactory
{
    IAuditReaderPatched Create();
}

public class AuditReaderPatchedFactory(ISession session) : IAuditReaderPatchedFactory
{
    public IAuditReaderPatched Create()
    {
        var sessionImpl = session as ISessionImplementor
                          ?? (ISessionImplementor)session.SessionFactory.GetCurrentSession();

        var listeners = sessionImpl.Listeners;

        var auditEventListener = listeners.PostInsertEventListeners.OfType<AuditEventListener>().SingleOrDefault() ?? listeners.PostUpdateEventListeners.OfType<AuditEventListener>().SingleOrDefault();

        if (null != auditEventListener)
        {
            return new AuditReaderPatched(auditEventListener.VerCfg, session, sessionImpl);
        }

        return AuditReaderFactoryPatched.NotImplemented;
    }
}
