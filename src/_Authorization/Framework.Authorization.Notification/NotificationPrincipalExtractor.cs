using System.Reflection;

using CommonFramework;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Attributes;
using SecuritySystem.HierarchicalExpand;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor : INotificationPrincipalExtractor
{
    private const string LevelsSeparator = "|";

    private const string LevelValueSeparator = ":";

    private readonly IServiceProvider serviceProvider;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly INotificationBasePermissionFilterSource notificationBasePermissionFilterSource;

    private readonly IRepository<Permission> permissionRepository;

    public NotificationPrincipalExtractor(
        IServiceProvider serviceProvider,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
        [DisabledSecurity] IRepository<Permission> permissionRepository)
    {
        this.serviceProvider = serviceProvider;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.notificationBasePermissionFilterSource = notificationBasePermissionFilterSource;
        this.permissionRepository = permissionRepository;
    }

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> preNotificationFilterGroups)
    {
        var notificationFilterGroups = preNotificationFilterGroups.ToArray();

        var startPermissionQ = this.permissionRepository.GetQueryable()
                                   .Where(this.notificationBasePermissionFilterSource.GetBasePermissionFilter(securityRoles))
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
        var genericMethod =

            notificationFilterGroup.SecurityContextType.IsHierarchical()

                ? this.GetType().GetMethod(nameof(this.ApplyNotificationFilterTypedHierarchical), BindingFlags.Instance | BindingFlags.NonPublic)
                : this.GetType().GetMethod(nameof(this.ApplyNotificationFilterTypedPlain), BindingFlags.Instance | BindingFlags.NonPublic);

        var method = genericMethod.MakeGenericMethod(notificationFilterGroup.SecurityContextType);

        return method.Invoke<IQueryable<PermissionLevelInfo>>(this, source, notificationFilterGroup);
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilterTypedHierarchical<TSecurityContext>(
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : ISecurityContext, IHierarchicalLevelObject
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? this.hierarchicalObjectExpanderFactory.Create(notificationFilterGroup.SecurityContextType).Expand(
                                        notificationFilterGroup.Idents,
                                        HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = this.serviceProvider.GetRequiredKeyedService<IRepository<TSecurityContext>>(nameof(SecurityRule.Disabled)).GetQueryable();

        return from permissionInfo in source

               let permission = permissionInfo.Permission

               let permissionSecurityContextItems = securityContextQ.Where(
                   securityContext => permission.Restrictions
                                                .Any(
                                                    fi => fi.SecurityContextType.Name == typeof(TSecurityContext).Name
                                                          && fi.SecurityContextId == securityContext.Id))


               let directLevel = permissionSecurityContextItems.Where(securityContext => expandedSecIdents.Contains(securityContext.Id))
                                                               .Select(secItem => (int?)secItem.DeepLevel).Max()
                                 ?? PriorityLevels.Access_Denied

               let grandLevel = grandAccess && permission.Restrictions.All(fi => fi.SecurityContextType.Name != typeof(TSecurityContext).Name)
                                    ? PriorityLevels.Grand_Access
                                    : PriorityLevels.Access_Denied

               let level = Math.Max(directLevel, grandLevel)

               where level != PriorityLevels.Access_Denied

               select new PermissionLevelInfo
                      {
                          Permission = permission,
                          LevelInfo = permissionInfo.LevelInfo
                                      + $"{LevelsSeparator}{typeof(TSecurityContext).Name}{LevelValueSeparator}{level}"
                      };
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilterTypedPlain<TSecurityContext>(
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : ISecurityContext
    {
        var expandedSecIdents = notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = this.serviceProvider.GetRequiredKeyedService<IRepository<TSecurityContext>>(nameof(SecurityRule.Disabled)).GetQueryable();

        return from permissionInfo in source

               let permission = permissionInfo.Permission

               let permissionSecurityContextItems = securityContextQ.Where(
                   securityContext => permission.Restrictions
                                                .Any(
                                                    fi => fi.SecurityContextType.Name == typeof(TSecurityContext).Name
                                                          && fi.SecurityContextId == securityContext.Id))


               let directLevel = permissionSecurityContextItems.Any(securityContext => expandedSecIdents.Contains(securityContext.Id)) ? 0 : PriorityLevels.Access_Denied

               let grandLevel = grandAccess && permission.Restrictions.All(fi => fi.SecurityContextType.Name != typeof(TSecurityContext).Name)
                                    ? PriorityLevels.Grand_Access
                                    : PriorityLevels.Access_Denied

               let level = Math.Max(directLevel, grandLevel)

               where level != PriorityLevels.Access_Denied

               select new PermissionLevelInfo
               {
                   Permission = permission,
                   LevelInfo = permissionInfo.LevelInfo
                                      + $"{LevelsSeparator}{typeof(TSecurityContext).Name}{LevelValueSeparator}{level}"
               };
    }
}
