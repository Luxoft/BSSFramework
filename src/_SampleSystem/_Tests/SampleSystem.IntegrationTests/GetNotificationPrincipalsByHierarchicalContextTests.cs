using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Authorization.Notification;
using Framework.Core;
using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class GetNotificationPrincipalsByHierarchicalContextTests : TestBase
{
    private BusinessUnitIdentityDTO rootBusinessUnit;

    private BusinessUnitIdentityDTO child_1_0_BusinessUnit;
    private BusinessUnitIdentityDTO child_1_1_BusinessUnit;
    private BusinessUnitIdentityDTO child_2_0_BusinessUnit;
    private BusinessUnitIdentityDTO child_2_1_BusinessUnit;

    private LocationIdentityDTO rootLocation;

    private LocationIdentityDTO child_1_0_Location;
    private LocationIdentityDTO child_1_1_Location;
    private LocationIdentityDTO child_2_0_Location;
    private LocationIdentityDTO child_2_1_Location;

    private ManagementUnitIdentityDTO rootManagementUnit;

    private ManagementUnitIdentityDTO child_1_0_ManagementUnit;
    private ManagementUnitIdentityDTO child_1_1_ManagementUnit;
    private ManagementUnitIdentityDTO child_2_0_ManagementUnit;
    private ManagementUnitIdentityDTO child_2_1_ManagementUnit;


    private OperationIdentityDTO searchNotificationOperation;

    private TestBusinessRole searchTestBusinessRole = new TestBusinessRole(nameof(searchNotificationRole));

    private BusinessRoleIdentityDTO searchNotificationRole;

    private string searchNotificationEmployeeLogin1 = nameof(searchNotificationEmployeeLogin1);
    private string searchNotificationEmployeeLogin2 = nameof(searchNotificationEmployeeLogin2);

    [TestInitialize]
    public void Setup()
    {
        this.rootBusinessUnit = this.DataHelper.SaveBusinessUnit(id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID);
        this.child_1_0_BusinessUnit = this.DataHelper.SaveBusinessUnit(parent: this.rootBusinessUnit);
        this.child_1_1_BusinessUnit = this.DataHelper.SaveBusinessUnit(parent: this.child_1_0_BusinessUnit);
        this.child_2_0_BusinessUnit = this.DataHelper.SaveBusinessUnit(parent: this.rootBusinessUnit);
        this.child_2_1_BusinessUnit = this.DataHelper.SaveBusinessUnit(parent: this.child_2_0_BusinessUnit);

        this.rootLocation = this.DataHelper.SaveLocation(id: DefaultConstants.LOCATION_PARENT_ID);
        this.child_1_0_Location = this.DataHelper.SaveLocation(parent: this.rootLocation);
        this.child_1_1_Location = this.DataHelper.SaveLocation(parent: this.child_1_0_Location);
        this.child_2_0_Location = this.DataHelper.SaveLocation(parent: this.rootLocation);
        this.child_2_1_Location = this.DataHelper.SaveLocation(parent: this.child_2_0_Location);

        this.rootManagementUnit = this.DataHelper.SaveManagementUnit(id: DefaultConstants.MANAGEMENT_UNIT_PARENT_COMPANY_ID);
        this.child_1_0_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_1_1_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.child_1_0_ManagementUnit);
        this.child_2_0_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_2_1_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.child_2_0_ManagementUnit);

        var authFacade = this.GetAuthControllerEvaluator();

        this.searchNotificationOperation = authFacade.Evaluate(
            c => c.GetSimpleOperationByName(nameof(SampleSystemSecurityOperationCode.SearchNotificationOperation))).Identity;

        this.searchNotificationRole = authFacade.Evaluate(
            c => c.SaveBusinessRole(
                new BusinessRoleStrictDTO
                {
                    Name = this.searchTestBusinessRole.GetRoleName(),
                    BusinessRoleOperationLinks = { new BusinessRoleOperationLinkStrictDTO { Operation = this.searchNotificationOperation } }
                }));
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.child_1_1_BusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.Direct);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(this.searchNotificationEmployeeLogin1);
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test2_Missed()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.Direct);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(0);
    }

    [TestMethod]
    public void GetPrincipals_DirectOrEmpty_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                null,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(this.searchNotificationEmployeeLogin1);
    }


    [TestMethod]
    public void GetPrincipals_DirectOrFirstParentOrEmpty_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(this.searchNotificationEmployeeLogin1);
    }

    [TestMethod]
    public void GetPrincipals_DirectOrFirstParentOrEmpty_Test2_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.rootManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(this.searchNotificationEmployeeLogin1);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void GetPrincipals_DirectOrFirstParentOrEmpty_Test3_Searched(bool swapPriority)
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.rootManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = swapPriority
                         ? this.GetNotificationPrincipalsByRoles(mbuChildFilter, fbuChildFilter, locChildFilter)
                         : this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(swapPriority ? this.searchNotificationEmployeeLogin2 : this.searchNotificationEmployeeLogin1);
    }

    [TestMethod]
    public void GetPrincipals_All_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemPermission(
                this.searchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                this.child_1_1_Location));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.All);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { this.child_1_1_Location.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, locChildFilter);

        // Assert
        result.Length.Should().Be(2);
        result.Should().Contain(this.searchNotificationEmployeeLogin1);
        result.Should().Contain(this.searchNotificationEmployeeLogin2);
    }

    private string[] GetNotificationPrincipalsByRoles(params NotificationFilterGroup[] notificationFilterGroups)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization
                              .NotificationPrincipalExtractor
                              .GetNotificationPrincipalsByRoles(new Guid[] { this.searchNotificationRole.Id }, notificationFilterGroups)
                              .ToArray(p => p.Name));
    }
}
