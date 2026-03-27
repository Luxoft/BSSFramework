using Framework.Application.Session;
using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public interface InHibSession : IDBSession
{
    IAuditReaderPatched AuditReader { get; }

    ISession NativeSession { get; }
}
