using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.Services;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor(
    IServiceProvider serviceProvider,
    IHierarchicalInfoSource hierarchicalInfoSource,
    INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
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

                                  group pair by pair.LevelInfo[notificationFilterGroup.SecurityContextType]

                                  into levelGroup

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
        var selector = this.GetPermissionLevelInfoExtractor(notificationFilterGroup.SecurityContextType).GetSelector(notificationFilterGroup);

        return from permissionLevelInfo in source.Select(selector)

               where permissionLevelInfo.Level != PriorityLevels.Access_Denied

               select new PermissionLevelInfo
                      {
                          Permission = permissionLevelInfo.Permission,
                          LevelInfo = permissionLevelInfo.LevelInfo
                                      + $"{LevelsSeparator}{notificationFilterGroup.SecurityContextType.Name}{LevelValueSeparator}{permissionLevelInfo.Level}"
                      };
    }

    private IPermissionLevelInfoExtractor GetPermissionLevelInfoExtractor(Type securityContextType)
    {
        var genericExtractorType = hierarchicalInfoSource.IsHierarchical(securityContextType)
                                       ? typeof(PermissionLevelInfoHierarchicalExtractor<>)
                                       : typeof(PermissionLevelInfoPlainExtractor<>);

        var extractorType = genericExtractorType.MakeGenericType(securityContextType);

        return (IPermissionLevelInfoExtractor)ActivatorUtilities.CreateInstance(serviceProvider, extractorType);
    }
}
