using CommonFramework;

using Framework.DomainDriven;
using SecuritySystem;

using SampleSystem.Domain.Projections;
using SampleSystem.Generated.DTO;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class ManualAndLegacyProjectionSecurityTests : TestBase
{
    private const string TestEmployeeLogin = "MALProjection SecurityTester";

    private EmployeeIdentityDTO TestEmp1;

    private EmployeeIdentityDTO TestEmp2;


    private BusinessUnitIdentityDTO bu1Ident;

    private BusinessUnitIdentityDTO bu2Ident;

    [TestInitialize]
    public void SetUp()
    {
        this.bu1Ident = this.DataHelper.SaveBusinessUnit();

        this.bu2Ident = this.DataHelper.SaveBusinessUnit();

        this.DataHelper.SaveEmployee(login: TestEmployeeLogin);

        this.AuthManager.For(TestEmployeeLogin).SetRole(new SampleSystemTestPermission(SampleSystemSecurityRole.SeManager, this.bu2Ident));

        this.TestEmp1 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu1Ident);

        this.TestEmp2 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident);
    }

    [TestMethod]
    public void TestManualEmployeeProjection_LoadedByManualDependencySecurity()
    {
        // Arrange
        var employeeQueryController = this.GetControllerEvaluator<EmployeeQueryController>(TestEmployeeLogin);

        // Act
        var items = employeeQueryController.Evaluate(c => c.GetTestManualEmployeeProjectionsByODataQueryString($"$filter={nameof(TestManualEmployeeProjection.CoreBusinessUnitId)} ne null")).Items;

        // Assert
        items.Count().Should().Be(1);
        items[0].Identity.Should().Be(this.TestEmp2);
    }

    [TestMethod]
    public void TestLegacyEmployeeProjection_LoadedByLegacyGenericSecurity()
    {
        // Arrange
        var employeeQueryController = this.GetControllerEvaluator<EmployeeQueryController>(TestEmployeeLogin);

        // Act
        var items = employeeQueryController.Evaluate(c => c.GetTestLegacyEmployeesByODataQueryString($"$filter={nameof(TestLegacyEmployee.BusinessUnit_Security)} ne null")).Items;

        // Assert
        items.Count().Should().Be(1);
        items[0].Identity.Should().Be(this.TestEmp2);
    }


    //[TestMethod]
    //public void TestLegacyEmployeeProjection_AccessorsResolved()
    //{
    //    // Arrange

    //    // Act
    //    var items = this.Evaluate(
    //        DBSessionMode.Read,
    //        TestEmployeeLogin,
    //        ctx =>
    //        {
    //            var bll = ctx.Logics.TestLegacyEmployeeFactory.Create(SecurityRule.View);

    //            return bll.GetListBy(v => v.BusinessUnit_Security != null)
    //                      .ToDictionary(v => v.Id, bll.SecurityProvider.GetAccessorData)
    //                      .ChangeValue(ctx.SecurityAccessorResolver.Resolve);
    //        });

    //    // Assert
    //    items.Count().Should().Be(1);
    //    var item = items.Single();
    //    item.Key.Should().Be(this.TestEmp2.Id);

    //    item.Value.Should().Contain(TestEmployeeLogin);
    //}
}
