using System.Linq.Expressions;
using System.Reflection;
using CommonFramework;
using CommonFramework.ExpressionEvaluate;
using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Microsoft.Extensions.DependencyInjection;
using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.HierarchicalExpand;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor(
    IServiceProvider serviceProvider,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
    IIdentityInfoSource identityInfoSource,
    [DisabledSecurity] IRepository<Permission> permissionRepository)
    : INotificationPrincipalExtractor
{
    private const string LevelsSeparator = "|";

    private const string LevelValueSeparator = ":";

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> preNotificationFilterGroups)
    {
        var notificationFilterGroups = preNotificationFilterGroups.ToArray();

        var startPermissionQ = permissionRepository.GetQueryable()
                                   .Where(notificationBasePermissionFilterSource.GetBasePermissionFilter(securityRoles))
                                   .Select(p => new PermissionLevelInfo { Permission = p, LevelInfo = "" });

        var principalInfoResult = notificationFilterGroups.Aggregate(startPermissionQ, this.ApplyNotificationFilter)
                                                          .Select(pair => new { pair.Permission.Principal, pair.LevelInfo })
                                                          .ToList();

        var typeDict = notificationFilterGroups.Select(g => g.SecurityContextType).ToDictionary(g => g.Name);

        var parsedLevelInfoResult =
            principalInfoResult
                .Select(principalInfo => new
                                         {
                                             principalInfo.Principal,
                                             LevelInfo = principalInfo.LevelInfo
                                                                      .Split(LevelsSeparator, StringSplitOptions.RemoveEmptyEntries)
                                                                      .Select(levelData => levelData.Split(LevelValueSeparator))
                                                                      .ToDictionary(
                                                                          levelParts => typeDict[levelParts[0]],
                                                                          levelParts => int.Parse(levelParts[1]))
                                         })
                .ToList();


        var optimalRequest = notificationFilterGroups.Aggregate(
            parsedLevelInfoResult,
            (state, notificationFilterGroup) =>
            {
                if (notificationFilterGroup.ExpandType == NotificationExpandType.All || !state.Any())
                {
                    return state;
                }
                else
                {
                    var request = from pair in state

                                  group pair by pair.LevelInfo[notificationFilterGroup.SecurityContextType] into levelGroup

                                  orderby levelGroup.Key descending

                                  select levelGroup;

                    return request.First().ToList();
                }
            });

        var result = optimalRequest.Select(pair => pair.Principal).Distinct().ToList();

        return result;
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilter(
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
    {
        return from permissionLevelInfo in source.Select(this.GetFullPermissionLevelInfoSelector(notificationFilterGroup))

               where permissionLevelInfo.Level != PriorityLevels.Access_Denied

               select new PermissionLevelInfo
                      {
                          Permission = permissionLevelInfo.Permission,
                          LevelInfo = permissionLevelInfo.LevelInfo
                                      + $"{LevelsSeparator}{notificationFilterGroup.SecurityContextType.Name}{LevelValueSeparator}{permissionLevelInfo.Level}"
                      };
    }

    private Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetFullPermissionLevelInfoSelector(
        NotificationFilterGroup notificationFilterGroup)
    {
        var genericMethod =

            notificationFilterGroup.SecurityContextType.IsHierarchical()

                ? this.GetType().GetMethod(
                    nameof(this.GetFullPermissionLevelInfoHierarchicalSelector),
                    BindingFlags.Instance | BindingFlags.NonPublic)!
                : this.GetType().GetMethod(
                    nameof(this.GetFullPermissionLevelInfoPlainSelector),
                    BindingFlags.Instance | BindingFlags.NonPublic)!;

        var method = genericMethod.MakeGenericMethod(notificationFilterGroup.SecurityContextType);

        return method.Invoke<Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>>>(this, notificationFilterGroup);
    }

    private Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetFullPermissionLevelInfoHierarchicalSelector<TSecurityContext>(
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : ISecurityContext, IHierarchicalLevelObject
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? hierarchicalObjectExpanderFactory.Create<Guid>(notificationFilterGroup.SecurityContextType).Expand(
                                        notificationFilterGroup.Idents,
                                        HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = serviceProvider
                               .GetRequiredKeyedService<IRepository<TSecurityContext>>(nameof(SecurityRule.Disabled))
                               .GetQueryable();

        var identityInfo = identityInfoSource.GetIdentityInfo<TSecurityContext, Guid>();

        return ExpressionEvaluateHelper
            .InlineEvaluate(ee =>

                                from permissionInfo in ExpressionHelper.GetIdentity<PermissionLevelInfo>()

                                let permission = permissionInfo.Permission

                                let permissionSecurityContextItems =
                                    securityContextQ.Where(securityContext => permission
                                                                              .Restrictions
                                                                              .Any(fi => fi.SecurityContextType.Name
                                                                                         == typeof(TSecurityContext).Name
                                                                                         && fi.SecurityContextId
                                                                                         == ee.Evaluate(
                                                                                             identityInfo.IdPath,
                                                                                             securityContext)))


                                let directLevel =
                                    permissionSecurityContextItems
                                        .Where(securityContext =>
                                                   expandedSecIdents.Contains(
                                                       ee.Evaluate(
                                                           identityInfo.IdPath,
                                                           securityContext)))
                                        .Select(secItem => (int?)secItem.DeepLevel).Max()
                                    ?? PriorityLevels.Access_Denied

                                let grandLevel =
                                    grandAccess
                                    && permission.Restrictions.All(fi => fi.SecurityContextType.Name
                                                                         != typeof(TSecurityContext).Name)
                                        ? PriorityLevels.Grand_Access
                                        : PriorityLevels.Access_Denied

                                let level = Math.Max(directLevel, grandLevel)

                                select new FullPermissionLevelInfo
                                       {
                                           Permission = permissionInfo.Permission, LevelInfo = permissionInfo.LevelInfo, Level = level
                                       });
    }

    private Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetFullPermissionLevelInfoPlainSelector<TSecurityContext>(
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : ISecurityContext
    {
        var expandedSecIdents = notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = serviceProvider.GetRequiredKeyedService<IRepository<TSecurityContext>>(nameof(SecurityRule.Disabled))
                                              .GetQueryable();

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

                                select new FullPermissionLevelInfo
                                       {
                                           Permission = permissionInfo.Permission, LevelInfo = permissionInfo.LevelInfo, Level = level
                                       });
    }
}
