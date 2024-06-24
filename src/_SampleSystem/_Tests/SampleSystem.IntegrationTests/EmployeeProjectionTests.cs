using FluentAssertions;

using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class EmployeeProjectionTests : TestBase
{
    private const string ProjectionPrincipalName = "Projection Tester";
    private const string TestEmployee1Login = "Test Employee 1";
    private const string TestEmployee2Login = "Test Employee 2";
    private const string TestEmployee3Login = "Test Employee 3";

    [TestInitialize]
    public void SetUp()
    {
        var buTypeId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

        var luxoftBuId = this.DataHelper.SaveBusinessUnit(
                                                          id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                                                          name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                                                          type: buTypeId);

        var costBuId = this.DataHelper.SaveBusinessUnit(
                                                        id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                                                        name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                                                        type: buTypeId,
                                                        parent: luxoftBuId);

        var profitBuId = this.DataHelper.SaveBusinessUnit(
                                                          id: DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID,
                                                          name: DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME,
                                                          type: buTypeId,
                                                          parent: luxoftBuId);

        this.DataHelper.SaveEmployee(login: ProjectionPrincipalName, coreBusinessUnit: costBuId);
        this.DataHelper.SaveEmployee(login: TestEmployee1Login, coreBusinessUnit: costBuId);
        this.DataHelper.SaveEmployee(login: TestEmployee2Login, coreBusinessUnit: profitBuId);
        this.DataHelper.SaveEmployee(login: TestEmployee3Login, coreBusinessUnit: costBuId);

        this.AuthHelper.SetUserRole(
            ProjectionPrincipalName,
            new SampleSystemTestPermission(
                SecurityRole.Administrator,
                new BusinessUnitIdentityDTO(DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID)));

        this.AuthHelper.SetUserRole(TestEmployee1Login, SampleSystemSecurityRole.TestRole1);
        this.AuthHelper.SetUserRole(TestEmployee3Login, SampleSystemSecurityRole.TestRole2);
    }

    [TestMethod]
    public void EmployeeProjectionTest()
    {
        // Arrange
        var identity = this.DataHelper.SaveEmployee(Guid.NewGuid());
        var controller = this.GetControllerEvaluator<EmployeeQueryController>();

        // Act
        var result = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString($"$filter=id eq GUID'{identity.Id}'"));
        var employee = result.Items.SingleOrDefault(e => e.Id == identity.Id);

        // Assert
        employee.Should().NotBeNull();
    }

    [TestMethod]
    public void EmployeeProjectionColumnSecurityTest()
    {
        // Arrange
        var expected = new[] { ProjectionPrincipalName, TestEmployee2Login }.ToArray(Maybe.Return);
        var controller = this.GetControllerEvaluator<EmployeeQueryController>(ProjectionPrincipalName);

        // Act
        var employees = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$filter=CoreBusinessUnit ne null"))
                               .Items;

        var logins = employees.Select(dto => dto.Login);

        // Assert
        logins.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void EmployeeProjectionSecurityTestNoAccess()
    {
        // Arrange
        var controller = this.GetControllerEvaluator<EmployeeQueryController>(TestEmployee1Login);

        // Act
        var employees = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$filter=CoreBusinessUnit ne null")).Items;

        // Assert
        var positions = employees.Select(dto => dto.PositionName);
        var logins = employees.Select(dto => dto.Login);
        positions.All(x => x.HasValue).Should().BeFalse();
        logins.All(x => x.HasValue).Should().BeTrue();
    }

    [TestMethod]
    public void EmployeeProjectionSecurityTestHasAccess()
    {
        // Arrange
        var controller = this.GetControllerEvaluator<EmployeeQueryController>(TestEmployee3Login);

        // Act
        var result = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$filter=CoreBusinessUnit ne null")).Items;
        var positions = result.Select(dto => dto.PositionName);

        // Assert
        positions.All(x => x.HasValue).Should().BeTrue();
    }

    [TestMethod]
    public void EmployeeProjectionSortingTest()
    {
        // Arrange
        var logins = new[] { "PST_AEmployee", "PST_BEmployee", "PST_ZEmployee" };
        logins.Foreach(login => this.DataHelper.SaveEmployee(login: login));

        var expected = logins.Reverse().ToArray(Maybe.Return);
        var controller = this.GetControllerEvaluator<EmployeeQueryController>();

        // Act
        var actual = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$orderby=Login desc"))
                               .Items.Where(e => e.Login.ToString().StartsWith("PST_")).Select(e => e.Login);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
