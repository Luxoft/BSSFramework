using FluentAssertions;

using Framework.Authorization.Notification;
using Framework.Core;
using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class GetNotificationPrincipalsByHierarchicalContextTests : TestBase
{
    private BusinessUnitIdentityDTO rootBusinessUnit;

    private BusinessUnitIdentityDTO child_1_0_BusinessUnit;
    private BusinessUnitIdentityDTO child_1_1_BusinessUnit;
    private BusinessUnitIdentityDTO child_2_0_BusinessUnit;
    private BusinessUnitIdentityDTO child_2_1_BusinessUnit;

    private ManagementUnitIdentityDTO rootManagementUnit;

    private ManagementUnitIdentityDTO child_1_0_ManagementUnit;
    private ManagementUnitIdentityDTO child_1_1_ManagementUnit;
    private ManagementUnitIdentityDTO child_2_0_ManagementUnit;
    private ManagementUnitIdentityDTO child_2_1_ManagementUnit;

    private EmployeeIdentityDTO rootEmployee;

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

        this.rootManagementUnit = this.DataHelper.SaveManagementUnit(id: DefaultConstants.MANAGEMENT_UNIT_PARENT_COMPANY_ID);
        this.child_1_0_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_1_1_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.child_1_0_ManagementUnit);
        this.child_2_0_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_2_1_ManagementUnit = this.DataHelper.SaveManagementUnit(parent: this.child_2_0_ManagementUnit);

        this.rootEmployee = this.DataHelper.SaveEmployee();
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_1_BusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.Direct);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);
        var employeeFilter = new NotificationFilterGroup(typeof(Employee), new[] { this.rootEmployee.Id }, NotificationExpandType.DirectOrFirstParent);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, employeeFilter);

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
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.Direct);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

        // Assert
        result.Length.Should().Be(0);
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test3_Missed()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrEmpty);
        var employeeChildFilter = new NotificationFilterGroup(typeof(Employee), new[] { this.rootEmployee.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, employeeChildFilter);

        // Assert
        result.Length.Should().Be(0);
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test4_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var employeeChildFilter = new NotificationFilterGroup(typeof(Employee), new[] { this.rootEmployee.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, employeeChildFilter);

        // Assert
        result.Length.Should().Be(1);
    }

    [TestMethod]
    public void GetPrincipals_DirectOrEmpty_Test1_Searched()
    {
        // Arrange
        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin1,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                null,
                this.child_1_1_ManagementUnit));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

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
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

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
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.rootManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

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
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.rootManagementUnit));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.DirectOrFirstParentOrEmpty);

        // Act
        var result = swapPriority
                         ? this.GetNotificationPrincipalsByRoles(mbuChildFilter, fbuChildFilter)
                         : this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

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
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit));

        this.AuthHelper.SetUserRole(
            this.searchNotificationEmployeeLogin2,
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { this.child_1_1_BusinessUnit.Id }, NotificationExpandType.All);
        var mbuChildFilter = new NotificationFilterGroup(typeof(ManagementUnit), new[] { this.child_1_1_ManagementUnit.Id }, NotificationExpandType.Direct);

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

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
                              //.GetNotificationPrincipalsByOperations(new Guid[] { this.searchNotificationOperation.Id }, notificationFilterGroups)
                              .GetNotificationPrincipalsByRoles([SampleSystemSecurityRole.SearchTestBusinessRole], notificationFilterGroups)
                              .ToArray(p => p.Name));
    }
}
