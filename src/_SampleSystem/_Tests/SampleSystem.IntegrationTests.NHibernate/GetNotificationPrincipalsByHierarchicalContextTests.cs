using Framework.Application;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.MU;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

using Xunit;

using SecuritySystem.Notification;
using SecuritySystem.Notification.Domain;

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

    public GetNotificationPrincipalsByHierarchicalContextTests()
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
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_1_BusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_ManagementUnit.Id]
                             };


        var employeeFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(Employee), ExpandType = NotificationExpandType.DirectOrFirstParent, Idents = [this.rootEmployee.Id]
                             };

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
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

        // Assert
        result.Length.Should().Be(0);
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test3_Missed()
    {
        // Arrange
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.DirectOrEmpty, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.DirectOrEmpty, Idents = [this.child_1_1_ManagementUnit.Id]
                             };


        var employeeChildFilter = new NotificationFilterGroup<Guid>
                                  {
                                      SecurityContextType = typeof(Employee), ExpandType = NotificationExpandType.Direct, Idents = [this.rootEmployee.Id]
                                  };

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, employeeChildFilter);

        // Assert
        result.Length.Should().Be(0);
    }

    [TestMethod]
    public void GetPrincipals_Direct_Test4_Searched()
    {
        // Arrange
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee));


        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };


        var employeeChildFilter = new NotificationFilterGroup<Guid>
                                  {
                                      SecurityContextType = typeof(Employee), ExpandType = NotificationExpandType.Direct, Idents = [this.rootEmployee.Id]
                                  };

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter, employeeChildFilter);

        // Assert
        result.Length.Should().Be(1);
    }

    [TestMethod]
    public void GetPrincipals_DirectOrEmpty_Test1_Searched()
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                null,
                this.child_1_1_ManagementUnit));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.DirectOrEmpty, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_ManagementUnit.Id]
                             };

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
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_ManagementUnit.Id]
                             };

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
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.rootManagementUnit));

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(this.searchNotificationEmployeeLogin1);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetPrincipals_DirectOrFirstParentOrEmpty_Test3_Searched(bool swapPriority)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.rootManagementUnit));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit));


        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

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
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit));

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit));


        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.All, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit), ExpandType = NotificationExpandType.Direct, Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = this.GetNotificationPrincipalsByRoles(fbuChildFilter, mbuChildFilter);

        // Assert
        result.Length.Should().Be(2);
        result.Should().Contain(this.searchNotificationEmployeeLogin1);
        result.Should().Contain(this.searchNotificationEmployeeLogin2);
    }

    private string[] GetNotificationPrincipalsByRoles(params NotificationFilterGroup[] notificationFilterGroups) =>

        this.Evaluate(
            DBSessionMode.Read,
            context => context.ServiceProvider.GetRequiredService<INotificationPrincipalExtractor<Framework.Authorization.Domain.Principal>>()
                              .GetPrincipalsAsync([SampleSystemSecurityRole.SearchTestBusinessRole], [..notificationFilterGroups])
                              .ToListAsync()
                              .GetAwaiter()
                              .GetResult()
                              .Select(p => p.Name)
                              .ToArray());
}
