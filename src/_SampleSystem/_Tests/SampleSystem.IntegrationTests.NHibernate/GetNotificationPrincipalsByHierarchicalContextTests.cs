using Anch.SecuritySystem.Notification;
using Anch.SecuritySystem.Notification.Domain;
using Anch.Testing.Xunit;

using Framework.Application;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.MU;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

public class GetNotificationPrincipalsByHierarchicalContextTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
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

    private readonly string searchNotificationEmployeeLogin1 = nameof(searchNotificationEmployeeLogin1);

    private readonly string searchNotificationEmployeeLogin2 = nameof(searchNotificationEmployeeLogin2);

    protected override async ValueTask InitializeAsync(CancellationToken ct)
    {
        this.rootBusinessUnit = this.DataManager.SaveBusinessUnit(id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID);
        this.child_1_0_BusinessUnit = this.DataManager.SaveBusinessUnit(parent: this.rootBusinessUnit);
        this.child_1_1_BusinessUnit = this.DataManager.SaveBusinessUnit(parent: this.child_1_0_BusinessUnit);
        this.child_2_0_BusinessUnit = this.DataManager.SaveBusinessUnit(parent: this.rootBusinessUnit);
        this.child_2_1_BusinessUnit = this.DataManager.SaveBusinessUnit(parent: this.child_2_0_BusinessUnit);

        this.rootManagementUnit = this.DataManager.SaveManagementUnit(id: DefaultConstants.MANAGEMENT_UNIT_PARENT_COMPANY_ID);
        this.child_1_0_ManagementUnit = this.DataManager.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_1_1_ManagementUnit = this.DataManager.SaveManagementUnit(parent: this.child_1_0_ManagementUnit);
        this.child_2_0_ManagementUnit = this.DataManager.SaveManagementUnit(parent: this.rootManagementUnit);
        this.child_2_1_ManagementUnit = this.DataManager.SaveManagementUnit(parent: this.child_2_0_ManagementUnit);

        this.rootEmployee = this.DataManager.SaveEmployee();
    }

    [AnchFact]
    public async Task GetPrincipals_Direct_Test1_Searched(CancellationToken ct)
    {
        // Arrange
        await this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_1_BusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee), ct);

        await this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };


        var employeeFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(Employee),
                                 ExpandType = NotificationExpandType.DirectOrFirstParent,
                                 Idents = [this.rootEmployee.Id]
                             };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter, employeeFilter], ct);

        // Assert
        Assert.Single(result);
        Assert.Equal(this.searchNotificationEmployeeLogin1, result.Single());
    }

    [AnchFact]
    public async Task GetPrincipals_Direct_Test2_Missed(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Empty(result);
    }

    [AnchFact]
    public async Task GetPrincipals_Direct_Test3_Missed(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.DirectOrEmpty,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };


        var employeeChildFilter = new NotificationFilterGroup<Guid>
                                  {
                                      SecurityContextType = typeof(Employee), ExpandType = NotificationExpandType.Direct, Idents = [this.rootEmployee.Id]
                                  };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter, employeeChildFilter], ct);

        // Assert
        Assert.Empty(result);
    }

    [AnchFact]
    public async Task GetPrincipals_Direct_Test4_Searched(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(
            this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit,
                employee: this.rootEmployee), ct);


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
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter, employeeChildFilter], ct);

        // Assert
        Assert.Single(result);
    }

    [AnchFact]
    public async Task GetPrincipals_DirectOrEmpty_Test1_Searched(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                null,
                this.child_1_1_ManagementUnit), ct);

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Single(result);
        Assert.Equal(this.searchNotificationEmployeeLogin1, result.Single());
    }


    [AnchFact]
    public async Task GetPrincipals_DirectOrFirstParentOrEmpty_Test1_Searched(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit),
                                 ExpandType = NotificationExpandType.DirectOrFirstParentOrEmpty,
                                 Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Single(result);
        Assert.Equal(this.searchNotificationEmployeeLogin1, result.Single());
    }

    [AnchFact]
    public async Task GetPrincipals_DirectOrFirstParentOrEmpty_Test2_Searched(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit), ct);

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.rootManagementUnit), ct);

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
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Single(result);
        Assert.Equal(this.searchNotificationEmployeeLogin1, result.Single());
    }

    [Theory]
    [AnchInlineData(false)]
    [AnchInlineData(true)]
    public async Task GetPrincipals_DirectOrFirstParentOrEmpty_Test3_Searched(bool swapPriority, CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.rootManagementUnit), ct);

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_0_ManagementUnit), ct);


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
                         ? await this.GetNotificationPrincipalsByRoles([mbuChildFilter, fbuChildFilter], ct)
                         : await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Single(result);
        Assert.Equal(swapPriority ? this.searchNotificationEmployeeLogin2 : this.searchNotificationEmployeeLogin1, result.Single());
    }

    [AnchFact]
    public async Task GetPrincipals_All_Test1_Searched(CancellationToken ct)
    {
        // Arrange
        this.AuthManager.For(this.searchNotificationEmployeeLogin1).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.child_1_0_BusinessUnit,
                this.child_1_1_ManagementUnit), ct);

        this.AuthManager.For(this.searchNotificationEmployeeLogin2).SetRoleAsync(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.SearchTestBusinessRole,
                this.rootBusinessUnit,
                this.child_1_1_ManagementUnit), ct);


        var fbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(BusinessUnit), ExpandType = NotificationExpandType.All, Idents = [this.child_1_1_BusinessUnit.Id]
                             };

        var mbuChildFilter = new NotificationFilterGroup<Guid>
                             {
                                 SecurityContextType = typeof(ManagementUnit),
                                 ExpandType = NotificationExpandType.Direct,
                                 Idents = [this.child_1_1_ManagementUnit.Id]
                             };

        // Act
        var result = await this.GetNotificationPrincipalsByRoles([fbuChildFilter, mbuChildFilter], ct);

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Contains(this.searchNotificationEmployeeLogin1, result);
        Assert.Contains(this.searchNotificationEmployeeLogin2, result);
    }

    private Task<string[]> GetNotificationPrincipalsByRoles(NotificationFilterGroup[] notificationFilterGroups, CancellationToken ct) =>

        this.EvaluateAsync(
            DBSessionMode.Read,
            async context => await context.ServiceProvider.GetRequiredService<INotificationPrincipalExtractor<Framework.Authorization.Domain.Principal>>()
                                          .GetPrincipalsAsync([SampleSystemSecurityRole.SearchTestBusinessRole], [.. notificationFilterGroups])
                                          .Select(p => p.Name)
                                          .ToArrayAsync(ct),
            ct);
}
