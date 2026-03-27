using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public interface INHibSession : IDBSession
{
    IAuditReaderPatched AuditReader { get; }

    ISession NativeSession { get; }
}
