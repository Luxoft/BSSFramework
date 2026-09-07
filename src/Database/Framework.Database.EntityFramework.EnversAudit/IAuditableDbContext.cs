using Anch.Core.Auth;

using Framework.Database.EntityFramework.Sessions;

namespace Framework.Database.EntityFramework.EnversAudit;

public interface IAuditableDbContext
{
    TimeProvider TimeProvider { get; }

    ICurrentUser CurrentUser { get; }

    EfCurrentRevisionState CurrentRevisionState { get; }
}
