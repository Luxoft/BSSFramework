using Anch.SecuritySystem.UserSource;

using Framework.Database.InlineAudit;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class CurrentUserAuditValueResolver<TUser>(ICurrentUserSource<TUser> currentUser) : IAuditValueResolver<TUser>
{
    public TUser GetCurrentValue() => currentUser.CurrentUser;
}
