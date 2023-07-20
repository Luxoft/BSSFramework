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

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { childBusinessUnit.Id }, NotificationExpandType.DirectOrFirstParent);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { childLocation.Id }, NotificationExpandType.DirectOrFirstParent);

        var operationId = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization.Logics.Principal.GetByName(employeeLogin)
                .Permissions.SelectMany(x => x.Role.BusinessRoleOperationLinks.Select(y => y.Operation))
                .First()
                .Id);
        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization
                              .NotificationPrincipalExtractor
                              .GetNotificationPrincipalsByOperations(new[] { operationId }, new[] { fbuChildFilter, locChildFilter })
                              .ToArray(p => p.Name));

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(employeeLogin);
    }
}
