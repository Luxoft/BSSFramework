using System.Reflection;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor : INotificationPrincipalExtractor
{
    private const string LevelsSeparator = "|";

    private const string LevelValueSeparator = ":";

    private readonly IServiceProvider serviceProvider;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly INotificationBasePermissionFilterSource notificationBasePermissionFilterSource;

    private readonly IRepositoryFactory<Permission> permissionRepositoryFactory;

    private readonly IRepositoryFactory<BusinessRole> businessRoleRepositoryFactory;

    public NotificationPrincipalExtractor(
        IServiceProvider serviceProvider,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IRepositoryFactory<BusinessRole> businessRoleRepositoryFactory)
    {
        this.serviceProvider = serviceProvider;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.notificationBasePermissionFilterSource = notificationBasePermissionFilterSource;
        this.permissionRepositoryFactory = permissionRepositoryFactory;
        this.businessRoleRepositoryFactory = businessRoleRepositoryFactory;
    }

    public IEnumerable<Principal> GetNotificationPrincipalsByOperations(
        Guid[] operationsIds,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        var roleIdents = this.businessRoleRepositoryFactory.Create().GetQueryable()
                             .Where(role => role.BusinessRoleOperationLinks.Any(link => operationsIds.Contains(link.Operation.Id)))
                             .ToArray(role => role.Id);

        return this.GetNotificationPrincipalsByRoles(roleIdents, notificationFilterGroups);
    }

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(
        Guid[] roleIdents,
        IEnumerable<NotificationFilterGroup> preNotificationFilterGroups)
    {
        var notificationFilterGroups = preNotificationFilterGroups.ToArray();

        var startPermissionQ = this.permissionRepositoryFactory.Create().GetQueryable()
                                   .Where(this.notificationBasePermissionFilterSource.GetBasePermissionFilter(roleIdents))
                                   .Select(p => new PermissionLevelInfo { Permission = p, LevelInfo = "" });

        var principalInfoResult = notificationFilterGroups.Aggregate(startPermissionQ, this.ApplyNotificationFilter)
                                                          .Select(pair => new { pair.Permission.Principal, pair.LevelInfo })
                                                          .ToList();



        var typeDict = notificationFilterGroups.Select(g => g.EntityType).ToDictionary(g => g.Name);

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

                                  group pair by pair.LevelInfo[notificationFilterGroup.EntityType] into levelGroup

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
        var genericMethod = this.GetType().GetMethod(
            nameof(this.ApplyNotificationFilterTyped),
            BindingFlags.Instance | BindingFlags.NonPublic);

        var method = genericMethod.MakeGenericMethod(notificationFilterGroup.EntityType);

        return method.Invoke<IQueryable<PermissionLevelInfo>>(this, source, notificationFilterGroup);
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilterTyped<TSecurityContext>(
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : IIdentityObject<Guid>, ISecurityContext, IHierarchicalLevelObject
    {

        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? this.hierarchicalObjectExpanderFactory.Create(notificationFilterGroup.EntityType).Expand(
                                        notificationFilterGroup.Idents,
                                        HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = this.serviceProvider.GetRequiredService<IRepositoryFactory<TSecurityContext>>().Create().GetQueryable();

        return from permissionInfo in source

               let permission = permissionInfo.Permission

               let permissionSecurityContextItems = securityContextQ.Where(
                   securityContext => permission.FilterItems
                                                .Any(
                                                    fi => fi.EntityType.Name == typeof(TSecurityContext).Name
                                                          && fi.ContextEntityId == securityContext.Id))


               let directLevel = permissionSecurityContextItems.Where(securityContext => expandedSecIdents.Contains(securityContext.Id))
                                                               .Select(secItem => (int?)secItem.DeepLevel).Max()
                                 ?? PriorityLevels.Access_Denied

               let grandLevel = grandAccess && permission.FilterItems.All(fi => fi.EntityType.Name != typeof(TSecurityContext).Name)
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
