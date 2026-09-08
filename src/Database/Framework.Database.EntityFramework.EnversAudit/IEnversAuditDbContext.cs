using Anch.Core.Auth;

namespace Framework.Database.EntityFramework.EnversAudit;

public interface IEnversAuditDbContext
{
    TimeProvider TimeProvider { get; }

    ICurrentUser CurrentUser { get; }

    EfCurrentRevisionState CurrentRevisionState { get; }
}
