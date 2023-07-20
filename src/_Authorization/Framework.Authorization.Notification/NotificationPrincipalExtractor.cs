using System.Reflection;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Notification;

public class NotificationPrincipalExtractor<TPersistentDomainObjectBase> : INotificationPrincipalExtractor
    where TPersistentDomainObjectBase : IIdentityObject<Guid>
{
    private readonly IServiceProvider serviceProvider;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly INotificationBasePermissionFilterSource notificationBasePermissionFilterSource;

    private readonly IRepository<Permission> permissionRepository;

    public NotificationPrincipalExtractor(
        IServiceProvider serviceProvider,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
        IRepositoryFactory<Permission> permissionRepositoryFactory)
    {
        this.serviceProvider = serviceProvider;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.notificationBasePermissionFilterSource = notificationBasePermissionFilterSource;
        this.permissionRepository = permissionRepositoryFactory.Create();
    }

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole(Guid[] roleIdents, IEnumerable<string> principalNames, Guid relatedRoleId) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        var startPermissionQ = this.permissionRepository.GetQueryable()
                                   .Where(this.notificationBasePermissionFilterSource.GetBasePermissionFilter(roleIdents))
                                   .Select(p => new PermissionLevelInfo { Permission = p, LevelInfo = "" });

        var permissionInfoResult = notificationFilterGroups.Aggregate(startPermissionQ, this.ApplyNotificationFilter).ToList();

        var principalInfoResult = permissionInfoResult.Select(pair => new { pair.Permission.Principal, pair.LevelInfo }).ToList();

        //var parsedResult = principalInfoResult.gr

        throw new NotImplementedException();
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
        where TSecurityContext : TPersistentDomainObjectBase, ISecurityContext, IHierarchicalLevelObject
    {

        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? this.hierarchicalObjectExpanderFactory.Create(notificationFilterGroup.EntityType).Expand(notificationFilterGroup.Idents, HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = this.serviceProvider.GetRequiredService<IRepositoryFactory<TSecurityContext>>().Create().GetQueryable();

        return from permissionInfo in source

               let permission = permissionInfo.Permission

               let permissionSecurityContextItems = securityContextQ.Where(
                   securityContext => permission.FilterItems
                                                 .Any(fi => fi.EntityType.Name == typeof(TSecurityContext).Name && fi.ContextEntityId == securityContext.Id))


               let directLevel = permissionSecurityContextItems.Where(securityContext => expandedSecIdents.Contains(securityContext.Id))
                                                                 .Select(secItem => (int?)secItem.DeepLevel).Max()
                                   ?? -2

               let grandLevel = grandAccess && permission.FilterItems.All(fi => fi.EntityType.Name != typeof(TSecurityContext).Name)
                                      ? -1
                                      : -2

               let level = Math.Max(directLevel, grandLevel)

               where level != -2

               select new PermissionLevelInfo
               {
                   Permission = permission,
                   LevelInfo = permissionInfo.LevelInfo + $"|{typeof(TSecurityContext).Name}:{level}"
               };
    }
}
