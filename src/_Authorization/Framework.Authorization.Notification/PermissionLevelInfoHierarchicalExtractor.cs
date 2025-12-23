using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;
using CommonFramework.IdentitySource;

using Framework.DomainDriven.Repository;
using Framework.Persistent;

using HierarchicalExpand;

using SecuritySystem.Attributes;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfoHierarchicalExtractor<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> repository,
    IIdentityInfoSource identityInfoSource,
    IHierarchicalObjectExpanderFactory hierarchicalObjectExpanderFactory,
    DeepLevelInfo<TSecurityContext> deepLevelInfo) : PermissionLevelInfoExtractor<TSecurityContext>(repository, identityInfoSource)
{
    protected override Expression<Func<IQueryable<TSecurityContext>, int>> GetDirectLevelExpression(NotificationFilterGroup notificationFilterGroup, IExpressionEvaluator ee)
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? hierarchicalObjectExpanderFactory.Create<Guid>(notificationFilterGroup.SecurityContextType).Expand(
                                        notificationFilterGroup.Idents,
                                        HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        return permissionSecurityContextItems => permissionSecurityContextItems
                                                 .Where(securityContext =>
                                                            expandedSecIdents.Contains(
                                                                ee.Evaluate(
                                                                    this.IdentityInfo.Id.Path,
                                                                    securityContext)))
                                                 .Select(secItem => (int?)ee.Evaluate(deepLevelInfo.Path, secItem)).Max()
                                                 ?? PriorityLevels.AccessDenied;
    }
}
