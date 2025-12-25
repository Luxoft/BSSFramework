using Framework.DomainDriven;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SecuritySystem;
using SecuritySystem.Validation;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SecurityContextRestrictionFilterTests : TestBase
{
    private static readonly SecurityRole DefaultSecurityRole = SampleSystemSecurityRole.DefaultRole;

    private static readonly SecurityRule DefaultRestrictionRule = DefaultSecurityRole.ToSecurityRule(
        customRestriction: SecurityPathRestriction.Default.Add<BusinessUnit>(filter: bu => bu.AllowedForFilterRole));

    private readonly string employeeLogin = "RestrictionFilterTests";

    private EmployeeIdentityDTO employee;

    private BusinessUnitIdentityDTO defaultBu;

    private BusinessUnitIdentityDTO buWithAllowedFilter;

    [TestInitialize]
    public void SetUp()
    {
        this.employee = this.DataHelper.SaveEmployee(login: this.employeeLogin);

        this.defaultBu = this.DataHelper.SaveBusinessUnit();

        this.buWithAllowedFilter = this.DataHelper.SaveBusinessUnit(allowedForFilterRole: true);
    }

    [TestMethod]
    public void CreatePermissionWithRestrictionFilter_ApplyInvalidBusinessUnit_ExceptionRaised()
    {
        // Arrange

        // Act
        var action = () => this.AuthManager.For(this.employee.Id).SetRole(
                         new SampleSystemTestPermission(SampleSystemSecurityRole.WithRestrictionFilterRole)
                         {
                             BusinessUnit = this.defaultBu
                         });

        // Assert
        action.Should().Throw<SecuritySystemValidationException>().And.Message.Should().Contain($"SecurityContext: '{this.defaultBu.Id}' denied by filter.");
    }

    [TestMethod]
    public void CreatePermissionWithRestrictionFilter_ApplyCorrectBusinessUnit_ExceptionNotRaised()
    {
        // Arrange

        // Act
        var action = () => this.AuthManager.For(this.employee.Id).SetRole(
                         new SampleSystemTestPermission(SampleSystemSecurityRole.WithRestrictionFilterRole)
                         {
                             BusinessUnit = this.buWithAllowedFilter
                         });

        // Assert
        action.Should().NotThrow();
    }


    [TestMethod]
    public void CreateCustomRestrictionRule_ApplyGrandPermission_OnlyCorrectBuFounded()
    {
        // Arrange
        this.AuthManager.For(this.employee.Id).SetRole(DefaultSecurityRole);

        // Act
        var allowedBuList = this.Evaluate(DBSessionMode.Read, this.employee.Id,
                                          ctx =>
                                          {
                                              var ee = ctx.CurrentEmployeeSource.CurrentUser;

                                              return ctx.Logics.BusinessUnitFactory.Create(DefaultRestrictionRule).GetSecureQueryable().Select(bu => bu.ToIdentityDTO()).ToList();
                                          });

        // Assert
        allowedBuList.Should().BeEquivalentTo([this.buWithAllowedFilter]);
    }

    [TestMethod]
    public void CreateCustomRestrictionRule_ApplySingleCorrectBU_OnlyCorrectBuFounded()
    {
        // Arrange
        this.AuthManager.For(this.employee.Id).SetRole(new SampleSystemTestPermission(DefaultSecurityRole) { BusinessUnits = [this.defaultBu, this.buWithAllowedFilter] });

        // Act
        var allowedBuList = this.Evaluate(DBSessionMode.Read, this.employee.Id,
            ctx => ctx.Logics.BusinessUnitFactory.Create(DefaultRestrictionRule).GetSecureQueryable().Select(bu => bu.ToIdentityDTO()).ToList());

        // Assert
        allowedBuList.Should().BeEquivalentTo([this.buWithAllowedFilter]);
    }

    [TestMethod]
    public void CreateCustomRestrictionRule_SearchAccessorsForGrandPermission_EmployeeFounded()
    {
        // Arrange
        this.AuthManager.For(this.employee.Id).SetRole(DefaultSecurityRole);

        // Act
        var accesors = this.Evaluate(DBSessionMode.Read, this.employee.Id,
                                     ctx =>
                                     {
                                         var bu = ctx.Logics.BusinessUnit.GetById(this.buWithAllowedFilter.Id, true);

                                         var accessorData = ctx
                                                            .SecurityService.GetSecurityProvider<BusinessUnit>(DefaultRestrictionRule)
                                                            .GetAccessorData(bu);

                                         return ctx.SecurityAccessorResolver.Resolve(accessorData).ToList();
                                     });

        // Assert
        accesors.Should().Contain(this.employeeLogin);
    }

    [TestMethod]
    public void CreateCustomRestrictionRule_SearchAccessorsForCorrectBU_EmployeeFounded()
    {
        // Arrange
        this.AuthManager.For(this.employee.Id).SetRole(new SampleSystemTestPermission(DefaultSecurityRole) { BusinessUnits = [this.defaultBu, this.buWithAllowedFilter] });

        // Act
        var accesors = this.Evaluate(DBSessionMode.Read, this.employee.Id,
                                          ctx =>
                                          {
                                              var bu = ctx.Logics.BusinessUnit.GetById(this.buWithAllowedFilter.Id, true);

                                              var accessorData = ctx
                                                                  .SecurityService.GetSecurityProvider<BusinessUnit>(DefaultRestrictionRule)
                                                                  .GetAccessorData(bu);

                                              return ctx.SecurityAccessorResolver.Resolve(accessorData).ToList();
                                          });

        // Assert
        accesors.Should().Contain(this.employeeLogin);
    }

    [TestMethod]
    public void CreateCustomRestrictionRule_SearchAccessorsForIncorrectBU_EmployeeNotFounded()
    {
        // Arrange
        this.AuthManager.For(this.employee.Id).SetRole(new SampleSystemTestPermission(DefaultSecurityRole) { BusinessUnits = [this.defaultBu, this.buWithAllowedFilter] });

        // Act
        var accesors = this.Evaluate(DBSessionMode.Read, this.employee.Id,
                                     ctx =>
                                     {
                                         var bu = ctx.Logics.BusinessUnit.GetById(this.defaultBu.Id, true);

                                         var accessorData = ctx
                                                            .SecurityService.GetSecurityProvider<BusinessUnit>(DefaultRestrictionRule)
                                                            .GetAccessorData(bu);

                                         return ctx.SecurityAccessorResolver.Resolve(accessorData).ToList();
                                     });

        // Assert
        accesors.Should().NotContainInConsecutiveOrder(this.employeeLogin);
    }
}
