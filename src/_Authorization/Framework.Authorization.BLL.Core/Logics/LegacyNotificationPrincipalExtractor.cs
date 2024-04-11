using Framework.Authorization.Domain;
using Framework.Core;

using System.Linq.Expressions;

using Framework.Authorization.Notification;
using Framework.DomainDriven.BLL;

namespace Framework.Authorization.BLL;

public class LegacyNotificationPrincipalExtractor : BLLContextContainer<IAuthorizationBLLContext>, INotificationPrincipalExtractor, INotificationBasePermissionFilterSource
{
    public LegacyNotificationPrincipalExtractor(IAuthorizationBLLContext context)
        : base(context)
    {
    }

    public Expression<Func<Permission, bool>> GetBasePermissionFilter(Guid[] roleIdents)
    {
        if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));

        var roles = this.Context.Logics.BusinessRole.GetListByIdents(roleIdents);

        var permissionQ = this.Context.AvailablePermissionSource.GetAvailablePermissionsQueryable(applyCurrentUser: false);

        return permission => roles.Contains(permission.Role) && permissionQ.Contains(permission);
    }

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));
        if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

        return this.GetInternalNotificationPrincipals(roleIdents, notificationFilterGroups).SelectMany(v => v).Distinct();
    }

    private IEnumerable<Principal[]> GetInternalNotificationPrincipals(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> baseNotificationFilterGroups)
    {
        if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));
        if (baseNotificationFilterGroups == null) throw new ArgumentNullException(nameof(baseNotificationFilterGroups));

        var baseNotificationFilter = this.GetBasePermissionFilter(roleIdents);

        foreach (var notificationFilterGroups in baseNotificationFilterGroups.PermuteByExpand())
        {
            var firstGroup = notificationFilterGroups.First();

            if (firstGroup.ExpandType.IsHierarchical())
            {
                var tailGroups = notificationFilterGroups.Skip(1).ToArray();

                var firstGroupEntityType = this.Context.GetEntityType(firstGroup.EntityType);

                var firstGroupExternalSource = this.Context.ExternalSource.GetTyped(firstGroupEntityType);

                foreach (var preExpandedIdent in firstGroup.Idents)
                {
                    var withExpandPrincipalsRequest = from expandedIdent in firstGroupExternalSource.GetSecurityEntitiesWithMasterExpand(preExpandedIdent)

                                                      let newFirtstGroup = new NotificationFilterGroup(firstGroup.EntityType, new[] { expandedIdent.Id }, firstGroup.ExpandType.WithoutHierarchical())

                                                      let principals = this.GetDirectNotificationPrincipals(baseNotificationFilter, new[] { newFirtstGroup }.Concat(tailGroups)).ToArray()

                                                      where principals.Any()

                                                      select principals;

                    Principal[] withExpandPrincipals;

                    if (firstGroup.ExpandType == NotificationExpandType.All)
                    {
                        withExpandPrincipals = withExpandPrincipalsRequest.SelectMany(z => z).ToArray();
                    }
                    else
                    {
                        withExpandPrincipals = withExpandPrincipalsRequest.FirstOrDefault();
                    }

                    if (withExpandPrincipals != null)
                    {
                        yield return withExpandPrincipals;
                    }
                }
            }
            else
            {
                yield return this.GetDirectNotificationPrincipals(baseNotificationFilter, notificationFilterGroups).ToArray();
            }
        }
    }

    private IEnumerable<Principal> GetDirectNotificationPrincipals(
            Expression<Func<Permission, bool>> baseNotificationFilter,
            IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (baseNotificationFilter == null) throw new ArgumentNullException(nameof(baseNotificationFilter));
        if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

        var totalFilter = notificationFilterGroups.Aggregate(baseNotificationFilter, (accumFilter, group) =>
        {
            var entityType = this.Context.GetEntityType(group.EntityType);

            var entityTypeFilter = this.GetDirectPermissionFilter(entityType, group.Idents, group.ExpandType.AllowEmpty());

            return accumFilter.BuildAnd(entityTypeFilter);
        });

        return this.GetNotificationPrincipalsByRoles(totalFilter);
    }

    private Expression<Func<Permission, bool>> GetDirectPermissionFilter(SecurityContextType entityType, IEnumerable<Guid> idetns, bool allowEmpty)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));
        if (idetns == null) throw new ArgumentNullException(nameof(idetns));

        return permission => permission.Restrictions.Any(fi => fi.SecurityContextType == entityType && idetns.Contains(fi.Entity.EntityId))
                             || (allowEmpty && permission.Restrictions.All(fi => fi.SecurityContextType != entityType));
    }

    private IEnumerable<Principal> GetNotificationPrincipalsByRoles(Expression<Func<Permission, bool>> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return this.Context.Logics.Permission.GetUnsecureQueryable()
                   .Where(filter)
                   .Select(permission => permission.Principal)
                   .Distinct();
    }
}
