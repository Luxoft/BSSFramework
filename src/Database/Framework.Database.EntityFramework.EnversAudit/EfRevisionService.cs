using Framework.Database.EntityFramework.Sessions;
using Framework.Database.EnversAudit;

namespace Framework.Database.EntityFramework.EnversAudit;

public class EfRevisionService(EfCurrentRevisionState currentRevisionState) : IRevisionService
{
    public long GetCurrentRevision() => currentRevisionState.CurrentRevision;

    public long GetMaxRevision() => throw new NotImplementedException();
}
