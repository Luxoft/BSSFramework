using Framework.Authorization.Domain;
using Framework.Core;

using System.Linq.Expressions;

using Framework.Authorization.Notification;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

public class LegacyNotificationPrincipalExtractor : BLLContextContainer<IAuthorizationBLLContext>, INotificationPrincipalExtractor
{
    private readonly INotificationBasePermissionFilterSource notificationBasePermissionFilterSource;

    public LegacyNotificationPrincipalExtractor(
        IAuthorizationBLLContext context,
        INotificationBasePermissionFilterSource notificationBasePermissionFilterSource)
        : base(context)
    {
        this.notificationBasePermissionFilterSource = notificationBasePermissionFilterSource;
    }

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

        var baseNotificationFilter = this.notificationBasePermissionFilterSource.GetBasePermissionFilter(securityRoles);

        foreach (var notificationFilterGroups in baseNotificationFilterGroups.PermuteByExpand())
        {
            var firstGroup = notificationFilterGroups.First();

            if (firstGroup.ExpandType.IsHierarchical())
            {
                var tailGroups = notificationFilterGroups.Skip(1).ToArray();

                var firstGroupSecurityContextType = this.Context.GetSecurityContextType(firstGroup.SecurityContextType);

                var firstGroupExternalSource = this.Context.ExternalSource.GetTyped(firstGroupSecurityContextType);

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
            var securityContextType = this.Context.GetSecurityContextType(group.SecurityContextType);

            var securityContextTypeFilter = this.GetDirectPermissionFilter(securityContextType, group.Idents, group.ExpandType.AllowEmpty());

            return accumFilter.BuildAnd(securityContextTypeFilter);
        });

        return this.GetNotificationPrincipalsByRoles(totalFilter);
    }

    private Expression<Func<Permission, bool>> GetDirectPermissionFilter(SecurityContextType securityContextType, IEnumerable<Guid> idetns, bool allowEmpty)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));
        if (idetns == null) throw new ArgumentNullException(nameof(idetns));

        return permission => permission.Restrictions.Any(fi => fi.SecurityContextType == securityContextType && idetns.Contains(fi.SecurityContextId))
                             || (allowEmpty && permission.Restrictions.All(fi => fi.SecurityContextType != securityContextType));
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
