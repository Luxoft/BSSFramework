using System.Linq.Expressions;

using CommonFramework.IdentitySource;

using Framework.DomainDriven.Repository;
using Framework.Persistent;

using HierarchicalExpand;

using SecuritySystem;
using SecuritySystem.Attributes;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfoHierarchicalExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    DeepLevelInfo<TSecurityContext> deepLevelInfo) : PermissionLevelInfoExtractor<TSecurityContext>(repository, identityInfoSource)
    where TSecurityContext : class, ISecurityContext
{
    protected override Expression<Func<IQueryable<TSecurityContext>, int>> GetDirectLevelExpression(NotificationFilterGroup notificationFilterGroup)
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? hierarchicalObjectExpanderFactory.Create<Guid>(notificationFilterGroup.SecurityContextType).Expand(
                                        notificationFilterGroup.Idents,
                                        HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var containsFilter = this.IdentityInfo.CreateContainsFilter(expandedSecIdents);

        return permissionSecurityContextItems => permissionSecurityContextItems
                                                 .Where(containsFilter)
                                                 .Select(deepLevelInfo.Path)
                                                 .Select(v => (int?)v)
                                                 .Max()
                                                 ?? PriorityLevels.AccessDenied;
    }
}
