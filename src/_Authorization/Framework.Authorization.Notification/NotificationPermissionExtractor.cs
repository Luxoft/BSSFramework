using CommonFramework.DependencyInjection;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.Attributes;
namespace Framework.Authorization.Notification;

public class NotificationPermissionExtractor(
    IServiceProxyFactory serviceProxyFactory,
    IHierarchicalInfoSource hierarchicalInfoSource,
    INotificationGeneralPermissionFilterFactory notificationGeneralPermissionFilterFactory,
    [DisabledSecurity] IRepository<Permission> permissionRepository)
    : INotificationPermissionExtractor
{
    private const string LevelsSeparator = "|";

    private const string LevelValueSeparator = ":";

    public async Task<List<Permission>> GetPermissionsAsync(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups,
        CancellationToken cancellationToken)
    {
        var cachedNotificationFilterGroups = notificationFilterGroups.ToArray();

        var startPermissionQ = permissionRepository.GetQueryable()
                                                   .Where(notificationGeneralPermissionFilterFactory.Create(securityRoles))
                                                   .Select(p => new PermissionLevelInfo { Permission = p, LevelInfo = "" });

        var permissionInfoResult = await cachedNotificationFilterGroups.Aggregate(startPermissionQ, this.ApplyNotificationFilter).GenericToListAsync(cancellationToken);

        var typeDict = cachedNotificationFilterGroups.Select(g => g.SecurityContextType).ToDictionary(g => g.Name);

        var parsedLevelInfoResult =
            permissionInfoResult
                .Select(principalInfo => new
                                         {
                                             principalInfo.Permission,
                                             LevelDict = principalInfo.LevelInfo
                                                                      .Split(LevelsSeparator, StringSplitOptions.RemoveEmptyEntries)
                                                                      .Select(levelData => levelData.Split(LevelValueSeparator))
                                                                      .ToDictionary(
                                                                          levelParts => typeDict[levelParts[0]],
                                                                          levelParts => int.Parse(levelParts[1]))
                                         })
                .ToList();


        var optimalRequest = cachedNotificationFilterGroups.Aggregate(
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

                                  group pair by pair.LevelDict[notificationFilterGroup.SecurityContextType]

                                  into levelGroup

                                  orderby levelGroup.Key descending

                                  select levelGroup;

                    return request.First().ToList();
                }
            });

        return await optimalRequest.Select(pair => pair.Permission).Distinct().ToAsyncEnumerable().ToListAsync(cancellationToken);
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilter(
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
    {
        var selector = this.GetPermissionLevelInfoExtractor(notificationFilterGroup.SecurityContextType).GetSelector(notificationFilterGroup);

        return from permissionLevelInfo in source.Select(selector)

               where permissionLevelInfo.Level != PriorityLevels.AccessDenied

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

        return serviceProxyFactory.Create<IPermissionLevelInfoExtractor>(extractorType);
    }
}
