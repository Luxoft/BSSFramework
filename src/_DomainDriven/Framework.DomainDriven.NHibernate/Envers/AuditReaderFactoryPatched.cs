using Framework.Core;

using NHibernate.Engine;
using NHibernate.Envers.Event;

namespace NHibernate.Envers.Patch;

public static class AuditReaderFactoryPatched
{
    public static IAuditReaderPatched GetAuditReader(this ISession session)
    {
        var sessionImpl = session as ISessionImplementor
                          ?? (ISessionImplementor)session.SessionFactory.GetCurrentSession();

        var listeners = sessionImpl.Listeners;

        var auditEventListener = listeners.PostInsertEventListeners.OfType<AuditEventListener>().SingleOrDefault() ?? listeners.PostUpdateEventListeners.OfType<AuditEventListener>().SingleOrDefault();

        if (null != auditEventListener)
        {
            return new AuditReaderPatched(auditEventListener.VerCfg, session, sessionImpl);
        }

        return LazyInterfaceImplementHelper.CreateNotImplemented<IAuditReaderPatched>();
    }
}
