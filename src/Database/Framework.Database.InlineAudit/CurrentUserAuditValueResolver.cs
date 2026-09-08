using Anch.Core.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.InlineAudit;

public class CurrentUserAuditValueResolver([FromKeyedServices(ICurrentUser.RawKey)] ICurrentUser currentUser) : IAuditValueResolver<string>
{
    public string GetCurrentValue() => currentUser.Name;
}
