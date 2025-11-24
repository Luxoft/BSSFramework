using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Framework.DomainDriven.Repository;

using SecuritySystem.Attributes;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfoPlainExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource) : IPermissionLevelInfoExtractor
{
    public Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetSelector(NotificationFilterGroup notificationFilterGroup)
    {
        var expandedSecIdents = notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = repository.GetQueryable();

        var identityInfo = identityInfoSource.GetIdentityInfo<TSecurityContext, Guid>();

        return ExpressionEvaluateHelper
            .InlineEvaluate(ee =>

                                from permissionInfo in ExpressionHelper.GetIdentity<PermissionLevelInfo>()

                                let permission = permissionInfo.Permission

                                let permissionSecurityContextItems =
                                    securityContextQ.Where(securityContext => permission.Restrictions
                                                                                        .Any(fi => fi.SecurityContextType.Name
                                                                                                   == typeof(TSecurityContext).Name
                                                                                                   && fi.SecurityContextId
                                                                                                   == ee.Evaluate(
                                                                                                       identityInfo.IdPath,
                                                                                                       securityContext)))


                                let directLevel =
                                    permissionSecurityContextItems.Any(securityContext => expandedSecIdents.Contains(
                                                                           ee.Evaluate(
                                                                               identityInfo.IdPath,
                                                                               securityContext)))
                                        ? 0
                                        : PriorityLevels.Access_Denied

                                let grandLevel = grandAccess
                                                 && permission.Restrictions.All(fi => fi.SecurityContextType.Name
                                                                                      != typeof(TSecurityContext).Name)
                                                     ? PriorityLevels.Grand_Access
                                                     : PriorityLevels.Access_Denied

                                let level = Math.Max(directLevel, grandLevel)

                                select new FullPermissionLevelInfo { Permission = permissionInfo.Permission, LevelInfo = permissionInfo.LevelInfo, Level = level });
    }
}
