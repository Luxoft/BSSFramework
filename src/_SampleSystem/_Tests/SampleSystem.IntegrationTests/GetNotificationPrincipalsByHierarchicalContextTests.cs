using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.Authorization.Notification;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class GetNotificationPrincipalsByHierarchicalContextTests : TestBase
{
    [TestMethod]
    public void GetPrincipalsWithPermissionToTwoRootUnits_By_Roles()
    {
        // Arrange
        var employeeLogin = "GetNotificationPrincipalsByHierarchicalContextTests_Employee";

        var rootBusinessUnit = this.DataHelper.SaveBusinessUnit(id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID);
        var childBusinessUnit = this.DataHelper.SaveBusinessUnit(parent: rootBusinessUnit);

        var rootLocation = this.DataHelper.SaveLocation(DefaultConstants.LOCATION_PARENT_ID);
        var childLocation = this.DataHelper.SaveLocation(parent: rootLocation);

        var permissionToRootUnits = new SampleSystemPermission(TestBusinessRole.SystemIntegration, childBusinessUnit, null, childLocation);
        this.AuthHelper.SetUserRole(employeeLogin, permissionToRootUnits);

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { childBusinessUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { childLocation.Id }, NotificationExpandType.Direct);
        var securityFilters = new[] { fbuChildFilter, locChildFilter };

        var roleId = this.EvaluateRead(
            c => c.Authorization.Logics.BusinessRole.GetByName(permissionToRootUnits.GetRoleName()).Id);

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization
                              .NotificationPrincipalExtractor
                              .GetNotificationPrincipalsByRoles(new[] { roleId }, securityFilters)
                              .ToArray(p => p.Name));

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(employeeLogin);
    }
}
