using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Framework.DomainDriven.Repository;

using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public abstract class PermissionLevelInfoExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource) : IPermissionLevelInfoExtractor
{
    protected readonly IdentityInfo<TSecurityContext, Guid> IdentityInfo = identityInfoSource.GetIdentityInfo<TSecurityContext, Guid>();

    protected abstract Expression<Func<IQueryable<TSecurityContext>, int>> GetDirectLevelExpression(NotificationFilterGroup notificationFilterGroup, IExpressionEvaluator ee);

    public Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetSelector(NotificationFilterGroup notificationFilterGroup)
    {
        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = repository.GetQueryable();

        return ExpressionEvaluateHelper
            .InlineEvaluate(ee =>
            {
                var getDirectLevelExpression = this.GetDirectLevelExpression(notificationFilterGroup, ee);

                return from permissionInfo in ExpressionHelper.GetIdentity<PermissionLevelInfo>()

                       let permission = permissionInfo.Permission

                       let permissionSecurityContextItems =
                           securityContextQ.Where(securityContext => permission.Restrictions
                                                                               .Any(fi => fi.SecurityContextType.Name
                                                                                          == typeof(TSecurityContext).Name
                                                                                          && fi.SecurityContextId
                                                                                          == ee.Evaluate(
                                                                                              this.IdentityInfo.IdPath,
                                                                                              securityContext)))


                       let directLevel = ee.Evaluate(getDirectLevelExpression, permissionSecurityContextItems)

                       let grandLevel =
                           grandAccess
                           && permission.Restrictions.All(fi => fi.SecurityContextType.Name
                                                                != typeof(TSecurityContext).Name)
                               ? PriorityLevels.GrandAccess
                               : PriorityLevels.AccessDenied

                       let level = Math.Max(directLevel, grandLevel)

                       select new FullPermissionLevelInfo { Permission = permissionInfo.Permission, LevelInfo = permissionInfo.LevelInfo, Level = level };
            });
    }
}
