using Framework.Authorization.Domain;
using Framework.Core;

using System.Linq.Expressions;

using Framework.Authorization.Notification;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

public class LegacyNotificationPrincipalExtractor(
    IAuthorizationBLLContext context,
    INotificationBasePermissionFilterSource notificationBasePermissionFilterSource,
    ISecurityContextSource securityContextSource)
    : BLLContextContainer<IAuthorizationBLLContext>(context), INotificationPrincipalExtractor
{
    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (securityRoles == null) throw new ArgumentNullException(nameof(securityRoles));
        if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

        return this.GetInternalNotificationPrincipals(securityRoles, notificationFilterGroups).SelectMany(v => v).Distinct();
    }

    private IEnumerable<Principal[]> GetInternalNotificationPrincipals(SecurityRole[] securityRoles, IEnumerable<NotificationFilterGroup> baseNotificationFilterGroups)
    {
        if (securityRoles == null) throw new ArgumentNullException(nameof(securityRoles));
        if (baseNotificationFilterGroups == null) throw new ArgumentNullException(nameof(baseNotificationFilterGroups));

        var baseNotificationFilter = notificationBasePermissionFilterSource.GetBasePermissionFilter(securityRoles);

        foreach (var notificationFilterGroups in baseNotificationFilterGroups.PermuteByExpand())
        {
            var firstGroup = notificationFilterGroups.First();

            if (firstGroup.ExpandType.IsHierarchical())
            {
                var tailGroups = notificationFilterGroups.Skip(1).ToArray();

                var firstGroupExternalSource = this.Context.ExternalSource.GetTyped(firstGroup.SecurityContextType);

                foreach (var preExpandedIdent in firstGroup.Idents)
                {
                    var withExpandPrincipalsRequest = from expandedIdent in firstGroupExternalSource.GetSecurityEntitiesWithMasterExpand(preExpandedIdent)

                                                      let newFirstGroup = new NotificationFilterGroup(firstGroup.SecurityContextType, [expandedIdent.Id], firstGroup.ExpandType.WithoutHierarchical())

                                                      let principals = this.GetDirectNotificationPrincipals(baseNotificationFilter, new[] { newFirstGroup }.Concat(tailGroups)).ToArray()

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
            var securityContextTypeFilter = this.GetDirectPermissionFilter(group.SecurityContextType, group.Idents, group.ExpandType.AllowEmpty());

            return accumFilter.BuildAnd(securityContextTypeFilter);
        });

        return this.GetNotificationPrincipalsByRoles(totalFilter);
    }

    private Expression<Func<Permission, bool>> GetDirectPermissionFilter(Type securityContextType, IEnumerable<Guid> idetns, bool allowEmpty)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));
        if (idetns == null) throw new ArgumentNullException(nameof(idetns));

        var securityContextTypeId = securityContextSource.GetSecurityContextInfo(securityContextType).Id;

        return permission => permission.Restrictions.Any(fi => fi.SecurityContextType.Id == securityContextTypeId && idetns.Contains(fi.SecurityContextId))
                             || (allowEmpty && permission.Restrictions.All(fi => fi.SecurityContextType.Id != securityContextTypeId));
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
