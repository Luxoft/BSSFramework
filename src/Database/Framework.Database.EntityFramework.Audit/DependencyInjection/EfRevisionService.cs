using Framework.Database.Audit;
using Framework.Database.EntityFramework.Sessions;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public class EfRevisionService(EfCurrentRevisionState currentRevisionState) : IRevisionService
{
    public long GetCurrentRevision() => currentRevisionState.CurrentRevision;

    public long GetMaxRevision() => throw new NotImplementedException();
}
