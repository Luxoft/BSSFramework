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
        var employeeLogin = "employee";

        var parentLocation = this.DataHelper.SaveLocation();
        var childLocation = this.DataHelper.SaveLocation(parent: parentLocation);

        var parentBusinessUnit = this.DataHelper.SaveBusinessUnit();
        var childBusinessUnit = this.DataHelper.SaveBusinessUnit(parent: parentBusinessUnit);

        var permissionToRootUnits = new SampleSystemPermission(TestBusinessRole.SystemIntegration, parentBusinessUnit, null, parentLocation);
        this.AuthHelper.SetUserRole(employeeLogin, permissionToRootUnits);

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { childBusinessUnit.Id }, NotificationExpandType.DirectOrFirstParent);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { childLocation.Id }, NotificationExpandType.DirectOrFirstParent);
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
