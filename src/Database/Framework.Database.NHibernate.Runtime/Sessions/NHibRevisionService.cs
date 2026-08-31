using Framework.Database.Audit;
using Framework.Database.NHibernate.DAL.Revisions;
using Framework.Database.NHibernate.Envers;

namespace Framework.Database.NHibernate.Sessions;

public class NHibRevisionService(IAuditReaderPatched auditReader) : IRevisionService
{

    /// <inheritdoc />
    public long GetCurrentRevision() => auditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;

    /// <inheritdoc />
    public long GetMaxRevision() => auditReader.GetMaxRevision();
}
