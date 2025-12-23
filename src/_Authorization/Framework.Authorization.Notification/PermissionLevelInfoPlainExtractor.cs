using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;
using CommonFramework.IdentitySource;

using Framework.DomainDriven.Repository;

using SecuritySystem.Attributes;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfoPlainExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource) : PermissionLevelInfoExtractor<TSecurityContext>(repository, identityInfoSource)
{
    protected override Expression<Func<IQueryable<TSecurityContext>, int>> GetDirectLevelExpression(NotificationFilterGroup notificationFilterGroup, IExpressionEvaluator ee)
    {
        var expandedSecIdents = notificationFilterGroup.Idents;

        return permissionSecurityContextItems => permissionSecurityContextItems.Any(securityContext => expandedSecIdents.Contains(
                                                                                        ee.Evaluate(
                                                                                            this.IdentityInfo.Id.Path,
                                                                                            securityContext)))
                                                     ? 0
                                                     : PriorityLevels.AccessDenied;
    }
}
