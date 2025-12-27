using System.Linq.Expressions;

using CommonFramework.IdentitySource;

using Framework.DomainDriven.Repository;

using SecuritySystem;
using SecuritySystem.Attributes;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfoPlainExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource) : PermissionLevelInfoExtractor<TSecurityContext>(repository, identityInfoSource)
    where TSecurityContext : class, ISecurityContext
{
    protected override Expression<Func<IQueryable<TSecurityContext>, int>> GetDirectLevelExpression(NotificationFilterGroup notificationFilterGroup)
    {
        var containsFilter = this.IdentityInfo.CreateContainsFilter(notificationFilterGroup.Idents);

        return permissionSecurityContextItems => permissionSecurityContextItems.Any(containsFilter) ? 0 : PriorityLevels.AccessDenied;
    }
}
