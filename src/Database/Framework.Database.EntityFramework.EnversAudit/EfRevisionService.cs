using Framework.Database.EnversAudit;

namespace Framework.Database.EntityFramework.EnversAudit;

public class EfRevisionService(EfCurrentRevisionState currentRevisionState, IEfAuditReader auditReader) : IRevisionService
{
    public long GetCurrentRevision() => currentRevisionState.CurrentRevision;

    public long GetMaxRevision() => auditReader.GetMaxRevision();
}
