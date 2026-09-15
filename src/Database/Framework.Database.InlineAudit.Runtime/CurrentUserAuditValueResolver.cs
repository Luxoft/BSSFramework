using Anch.Core.Auth;

namespace Framework.Database.InlineAudit;

public class CurrentUserAuditValueResolver(ICurrentUser currentUser) : IAuditValueResolver<string>
{
    public string GetCurrentValue() => currentUser.Name;
}
