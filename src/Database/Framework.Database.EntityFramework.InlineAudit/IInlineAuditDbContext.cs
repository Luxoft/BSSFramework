using Anch.Core.Auth;

namespace Framework.Database.EntityFramework.InlineAudit;

public interface IInlineAuditDbContext
{
    TimeProvider TimeProvider { get; }

    ICurrentUser CurrentUser { get; }
}
