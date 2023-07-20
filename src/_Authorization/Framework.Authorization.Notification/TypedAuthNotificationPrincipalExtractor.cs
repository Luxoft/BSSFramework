using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.Notification;

public class TypedAthNotificationPrincipalExtractor<TTypedAuthPermission> : INotificationPrincipalExtractor
    where TTypedAuthPermission : IIdentityObject<Guid>
{
    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    private readonly IRepository<Permission> permissionRepository;

    private readonly IRepository<TTypedAuthPermission> typedAuthPermissionRepository;

    public TypedAthNotificationPrincipalExtractor(
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IRepositoryFactory<TTypedAuthPermission> typedAuthPermissionRepositoryFactory)
    {
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.permissionRepository = permissionRepositoryFactory.Create();
        this.typedAuthPermissionRepository = typedAuthPermissionRepositoryFactory.Create();
    }

    public Expression<Func<Permission, bool>> GetRoleBaseNotificationFilter(Guid[] roleIdents) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRoles(Guid[] roleIdents, IEnumerable<NotificationFilterGroup> notificationFilterGroups) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByOperations(Guid[] operationsIds, IEnumerable<NotificationFilterGroup> notificationFilterGroups) => throw new NotImplementedException();

    public IEnumerable<Principal> GetNotificationPrincipalsByRelatedRole(Guid[] roleIdents, IEnumerable<string> principalNames, Guid relatedRoleId)
    {
        var basePermissionQ = this.permissionRepository.GetQueryable();

        var basePermissionFilter = this.GetRoleBaseNotificationFilter(roleIdents);

        var basePermissionPreFilteredQ = basePermissionQ.Where(basePermissionFilter).Select(permission => permission.Id);

        var typedPermissionQ = this.typedAuthPermissionRepository.GetQueryable();

        var preFiltered = typedPermissionQ.Where(typedPermission => basePermissionPreFilteredQ.Contains(typedPermission.Id));


        var typedPermissionRequestWithBu = WithTypedAuthPermissionFilter(
            context.HierarchicalObjectExpanderFactory,
            preFiltered,
            pair => pair,
            v => v.BusinessUnitItems.Select(item => item.ContextEntity),
            fbuChildFilter,
            (typedPermission, buLevel) => new { typedPermission, BuLevel = buLevel },
            pair => pair.BuLevel);

        var rr = typedPermissionRequestWithBu.ToList();

        var typedPermissionRequestWithLoc = WithTypedAuthPermissionFilter(
            context.HierarchicalObjectExpanderFactory,
            typedPermissionRequestWithBu,
            pair => pair.typedPermission,
            v => v.LocationItems.Select(item => item.ContextEntity),
            locChildFilter,
            (pair, locLevel) => new { pair.typedPermission, pair.BuLevel, LocLecel = locLevel },
            pair => pair.LocLecel);

        var typedPermissions = typedPermissionRequestWithLoc.ToList();
    }

    private IQueryable<TResult> WithTypedAuthPermissionFilter<TSource, TItem, TResult>(
        IQueryable<TSource> source,
        Expression<Func<TSource, TTypedAuthPermission>> permissionPath,
        Expression<Func<TTypedAuthPermission, IEnumerable<TItem>>> securityItemsPath,
        NotificationFilterGroup notificationFilterGroup,
        Expression<Func<TSource, int, TResult>> resultSelector,
        Expression<Func<TResult, int>> levelSelector)
        where TItem : PersistentDomainObjectBase, IHierarchicalLevelObject, ISecurityContext
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? this.hierarchicalObjectExpanderFactory.Create(typeof(TItem)).Expand(notificationFilterGroup.Idents, HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var selector =

            from typedPermissionSource in ExpressionHelper.GetIdentity<TSource>()

            let typedPermission = permissionPath.Eval(typedPermissionSource)

            let directLevel = securityItemsPath.Eval(typedPermission)
                                               .Where(secItem => expandedSecIdents.Contains(secItem.Id))
                                               .Select(secItem => (int?)secItem.DeepLevel).Max()

                              ?? PriorityLevels.Access_Denied

            let grandLevel = grandAccess && !securityItemsPath.Eval(typedPermission).Any()
                                 ? PriorityLevels.Grand_Access
                                 : PriorityLevels.Access_Denied

            let level = Math.Max(directLevel, grandLevel)

            select resultSelector.Eval(typedPermissionSource, level);


        var filter =

            from level in levelSelector

            select level != PriorityLevels.Access_Denied;

        return source.Select(selector.ExpandConst().InlineEval())
                     .Where(filter.ExpandConst().InlineEval());
    }
}
