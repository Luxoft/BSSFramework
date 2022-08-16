using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public partial class PermissionBLL
    {
        private Expression<Func<Permission, bool>> GetRoleNotificationFilter(Guid[] roleIdents)
        {
            if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));

            var roles = this.Context.Logics.BusinessRole.GetListByIdents(roleIdents);
            var expandedRoles = this.Context.Logics.BusinessRole.GetParents(roles).ToArray();

            return new AvailablePermissionFilter(this.Context.DateTimeService, null).ToFilterExpression()
                                                      .BuildAnd(permission => expandedRoles.Contains(permission.Role));
        }

        private IEnumerable<Principal> GetNotificationPrincipalsByRoles(Expression<Func<Permission, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return this.GetUnsecureQueryable()
                       .Where(filter)
                       .Select(permission => permission.Principal)
                       .Distinct();
        }

        public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents)
        {
            if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));

            return this.GetNotificationPrincipalsByRoles(this.GetRoleNotificationFilter(roleIdents));
        }

        public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, [NotNull]IEnumerable<NotificationFilterGroup> notificationFilterGroups)
        {
            if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));
            if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

            return this.GetInternalNotificationPrincipals(roleIdents, notificationFilterGroups).SelectMany(v => v).Distinct();
        }

        private IEnumerable<Principal[]> GetInternalNotificationPrincipals(Guid[] roleIdents, [NotNull]IEnumerable<NotificationFilterGroup> baseNotificationFilterGroups)
        {
            if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));
            if (baseNotificationFilterGroups == null) throw new ArgumentNullException(nameof(baseNotificationFilterGroups));

            var baseNotificationFilter = this.GetRoleNotificationFilter(roleIdents);

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

        private Expression<Func<Permission, bool>> GetDirectPermissionFilter(EntityType entityType, IEnumerable<Guid> idetns, bool allowEmpty)
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            if (idetns == null) throw new ArgumentNullException(nameof(idetns));

            return permission => permission.FilterItems.Any(fi => fi.EntityType == entityType && idetns.Contains(fi.Entity.EntityId))
                              || (allowEmpty && permission.FilterItems.All(fi => fi.EntityType != entityType));
        }

        public IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole(Guid[] roleIdents, IEnumerable<string> principalNames, Guid relatedRoleId)
        {
            if (roleIdents == null) throw new ArgumentNullException(nameof(roleIdents));
            if (principalNames == null) throw new ArgumentNullException(nameof(principalNames));
            if (relatedRoleId.IsDefault()) throw new System.ArgumentException("relatedRoleId");

            var roles = this.Context.Logics.BusinessRole.GetListByIdents(roleIdents).ToArray();
            var relatedRole = this.Context.Logics.BusinessRole.GetById(relatedRoleId, true);
            var principals = this.Context.Logics.Principal.GetListBy(z => principalNames.Contains(z.Name)).ToArray();

            return this.GetNotificationPrincipalsByRelatedRole(roles, principals, relatedRole);
        }

        private IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole([NotNull] IList<BusinessRole> roles, [NotNull] IEnumerable<Principal> principals, [NotNull] BusinessRole relatedRole)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));
            if (principals == null) throw new ArgumentNullException(nameof(principals));
            if (relatedRole == null) throw new ArgumentNullException(nameof(relatedRole));

            Expression<Func<Permission, bool>> filterExpression = z => z.Role == relatedRole;

            var filterByPermFilters = this.GetPermissionsForRoles(roles, principals).BuildOr(perm =>
            {
                return perm.FilterItems.GroupBy(f => f.EntityType).BuildAnd(entityFilter =>
                {
                    var entityIds = entityFilter.Select(z => z.Entity.EntityId).ToList();

                    Expression<Func<Permission, bool>> addFilterExpression =
                        z => z.FilterItems.Any(e => e.EntityType == entityFilter.Key && entityIds.Contains(e.Entity.EntityId))
                        || z.FilterItems.All(e => e.EntityType != entityFilter.Key);

                    return addFilterExpression;
                });
            });

            var today = this.Context.DateTimeService.Today;

            var query = filterExpression.BuildAnd(filterByPermFilters).BuildAnd(p => p.Status == PermissionStatus.Approved && p.Period.Contains(today));

            return this.GetListBy(query, f => f.Select(l => l.Principal)).Select(z => z.Principal).Distinct();
        }

        private IEnumerable<Permission> GetPermissionsForRoles([NotNull] ICollection<BusinessRole> roles, [NotNull] IEnumerable<Principal> principals)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));
            if (principals == null) throw new ArgumentNullException(nameof(principals));

            if (!roles.Any()) return Enumerable.Empty<Permission>();

            var principalIds = principals.Select(z => z.Id).ToList();

            var allRoles = this.Context.Logics.BusinessRole.GetParents(roles).ToArray();

            var today = this.Context.DateTimeService.Today;

            var result = this.GetListBy(z => z.Status == PermissionStatus.Approved && allRoles.Contains(z.Role) && principalIds.Contains(z.Principal.Id) && z.Period.Contains(today));

            return result;
        }

        public IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups)
        {
            if (operationsIds == null) throw new ArgumentNullException(nameof(operationsIds));
            if (notificationFilterGroups == null) throw new ArgumentNullException(nameof(notificationFilterGroups));

            var operations = this.Context.Logics.Operation.GetListByIdents(operationsIds).ToArray();

            var roleIdents = this.Context.Logics.BusinessRole.GetListBy(role => role.BusinessRoleOperationLinks.Any(link => operations.Contains(link.Operation))).ToArray(role => role.Id);

            return this.GetNotificationPrincipalsByRoles(roleIdents, notificationFilterGroups);
        }
    }
}
