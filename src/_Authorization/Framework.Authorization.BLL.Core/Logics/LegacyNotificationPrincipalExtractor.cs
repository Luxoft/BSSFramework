using Framework.Authorization.Domain;

using System.Linq.Expressions;

using CommonFramework;

using Framework.Authorization.Notification;
using Framework.DomainDriven.Repository;

using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.ExternalSystem.SecurityContextStorage;

namespace Framework.Authorization.BLL;

public class LegacyNotificationPrincipalExtractor(
    ISecurityContextStorage securityContextStorage,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    INotificationGeneralPermissionFilterFactory notificationGeneralPermissionFilterFactory,
    ISecurityContextInfoSource securityContextInfoSource) : INotificationPermissionExtractor
{
    public async Task<List<Permission>> GetPermissionsAsync(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups,
        CancellationToken cancellationToken)
    {
        return await this.GetInternalNotificationPermissions(securityRoles, notificationFilterGroups).SelectMany().Distinct().ToAsyncEnumerable().ToListAsync(cancellationToken);
    }

    private IEnumerable<Permission[]> GetInternalNotificationPermissions(SecurityRole[] securityRoles, IEnumerable<NotificationFilterGroup> baseNotificationFilterGroups)
    {
        var baseNotificationFilter = notificationGeneralPermissionFilterFactory.Create(securityRoles);

        foreach (var notificationFilterGroups in baseNotificationFilterGroups.PermuteByExpand())
        {
            var firstGroup = notificationFilterGroups.First();

            if (firstGroup.ExpandType.IsHierarchical())
            {
                var tailGroups = notificationFilterGroups.Skip(1).ToArray();

                var firstGroupExternalSource = securityContextStorage.GetTyped<Guid>(firstGroup.SecurityContextType);

                foreach (var preExpandedIdent in firstGroup.Idents)
                {
                    var withExpandPrincipalsRequest = from expandedIdent in firstGroupExternalSource.GetSecurityContextsWithMasterExpand(preExpandedIdent)

                                                      let newFirstGroup = new NotificationFilterGroup(firstGroup.SecurityContextType, [expandedIdent.Id], firstGroup.ExpandType.WithoutHierarchical())

                                                      let permissions = this.GetDirectNotificationPermissions(baseNotificationFilter, new[] { newFirstGroup }.Concat(tailGroups)).ToArray()

                                                      where permissions.Any()

                                                      select permissions.ToArray();

                    if (firstGroup.ExpandType == NotificationExpandType.All)
                    {
                        yield return withExpandPrincipalsRequest.SelectMany().ToArray();
                    }
                    else
                    {
                        var withExpandPermissions = withExpandPrincipalsRequest.FirstOrDefault();

                        if (withExpandPermissions != null)
                        {
                            yield return withExpandPermissions;
                        }
                    }
                }
            }
            else
            {
                yield return this.GetDirectNotificationPermissions(baseNotificationFilter, notificationFilterGroups).ToArray();
            }
        }
    }

    private IEnumerable<Permission> GetDirectNotificationPermissions(
            Expression<Func<Permission, bool>> baseNotificationFilter,
            IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (baseNotificationFilter == null) throw new ArgumentNullException(nameof(baseNotificationFilter));
        if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

        var totalFilter = notificationFilterGroups.Aggregate(baseNotificationFilter, (accumFilter, group) =>
        {
            var securityContextTypeFilter = this.GetDirectPermissionFilter(group.SecurityContextType, group.Idents, group.ExpandType.AllowEmpty());

            return accumFilter.BuildAnd(securityContextTypeFilter);
        });

        return this.GetNotificationPermissionByRoles(totalFilter);
    }

    private Expression<Func<Permission, bool>> GetDirectPermissionFilter(Type securityContextType, IEnumerable<Guid> idetns, bool allowEmpty)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));
        if (idetns == null) throw new ArgumentNullException(nameof(idetns));

        var securityContextTypeId = securityContextInfoSource.GetSecurityContextInfo(securityContextType).Id;

        return permission => permission.Restrictions.Any(fi => fi.SecurityContextType.Id == securityContextTypeId && idetns.Contains(fi.SecurityContextId))
                             || (allowEmpty && permission.Restrictions.All(fi => fi.SecurityContextType.Id != securityContextTypeId));
    }

    private IEnumerable<Permission> GetNotificationPermissionByRoles(Expression<Func<Permission, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return permissionRepository.GetQueryable().Where(filter).Distinct();
    }
}
