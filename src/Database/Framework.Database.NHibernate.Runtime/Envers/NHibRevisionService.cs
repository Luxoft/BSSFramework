using Framework.Database.EnversAudit;
using Framework.Database.NHibernate.DAL.Revisions;

namespace Framework.Database.NHibernate.Envers;

public class NHibRevisionService(IAuditReaderPatched auditReader) : IRevisionService
{
    /// <inheritdoc />
    public long GetCurrentRevision() => auditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;

    /// <inheritdoc />
    public long GetMaxRevision() => auditReader.GetMaxRevision();
}
