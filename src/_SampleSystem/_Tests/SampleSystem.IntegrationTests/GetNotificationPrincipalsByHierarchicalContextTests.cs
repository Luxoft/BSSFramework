using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Authorization.Notification;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.TypedAuth;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class GetNotificationPrincipalsByHierarchicalContextTests : TestBase
{
    [TestMethod]
    public void GetPrincipalsWithPermissionToTwoRootUnits_By_Operations()
    {
        // Arrange
        var employeeLogin = "employee";

        var parentLocation = new LocationIdentityDTO(DefaultConstants.LOCATION_PARENT_ID);
        var childLocation = this.DataHelper.SaveLocation(parent: parentLocation);

        var parentBusinessUnit = this.DataHelper.SaveBusinessUnit(id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID);
        var childBusinessUnit = this.DataHelper.SaveBusinessUnit(parent: parentBusinessUnit);

        var permissionToRootUnits = new SampleSystemPermission(TestBusinessRole.SystemIntegration, parentBusinessUnit, null, parentLocation);
        this.AuthHelper.SetUserRole(employeeLogin, permissionToRootUnits);

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { childBusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { childLocation.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);

        var operationId = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization.Logics.Principal.GetByName(employeeLogin)
                .Permissions.SelectMany(x => x.Role.BusinessRoleOperationLinks.Select(y => y.Operation))
                .First()
                .Id);
        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization.NotificationPrincipalExtractor
                              .GetNotificationPrincipalsByOperations(new[] { operationId }, new[] { fbuChildFilter, locChildFilter })
                .ToArray());


        this.Evaluate(
            DBSessionMode.Read,
            context =>
            {
                var role = context.Authorization.Logics.BusinessRole.GetByName(TestBusinessRole.SystemIntegration.GetRoleName());

                var basePermissionQ = context.Authorization.Logics.Permission.GetUnsecureQueryable();

                var basePermissionFilter = context.Authorization.NotificationPrincipalExtractor.GetRoleBaseNotificationFilter(new[] { role.Id });

                var basePermissionPreFilteredQ = basePermissionQ.Where(basePermissionFilter).Select(permission => permission.Id);

                var typedPermissionQ = context.Logics.Default.Create<TypedAuthPermission>().GetUnsecureQueryable();

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

                //var permissions = context.Authorization.Logics.Permission.GetListByIdents(typedPermissions.Select(pair => pair.Id));

                return;
            });

        // Assert
        result.Select(x => x.Name).Should().Contain(employeeLogin);
    }

    private static IQueryable<TResult> WithTypedAuthPermissionFilter<TSource, TItem, TResult>(
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IQueryable<TSource> source,
        Expression<Func<TSource, TypedAuthPermission>> permissionPath,
        Expression<Func<TypedAuthPermission, IEnumerable<TItem>>> securityItemsPath,
        NotificationFilterGroup notificationFilterGroup,
        Expression<Func<TSource, int, TResult>> resultSelector,
        Expression<Func<TResult, int>> levelSelector)
        where TItem : PersistentDomainObjectBase, IHierarchicalLevelObject, ISecurityContext
    {
        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? hierarchicalObjectExpanderFactory.Create(typeof(TItem)).Expand(notificationFilterGroup.Idents, HierarchicalExpandType.Parents)
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

public static class PriorityLevels
{
    public const int Grand_Access = -1;

    public const int Access_Denied = -2;
}
