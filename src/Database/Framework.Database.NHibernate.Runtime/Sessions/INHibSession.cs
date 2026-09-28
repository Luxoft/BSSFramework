using Framework.Database.NHibernate.EnversAudit;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public interface INHibSession : IDBSession<ISession>
{
    IAuditReaderPatched AuditReader { get; }
}
