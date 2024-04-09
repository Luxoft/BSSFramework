using System.Linq.Expressions;
using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
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

        this.Evaluate(
                      DBSessionMode.Write,
                      context =>
                      {
                          var authContext = context.Authorization;

                          var principalBll = authContext.Logics.Principal;
                          var principal1 = principalBll.GetByNameOrCreate(ProjectionPrincipalName, true);
                          var principal2 = principalBll.GetByNameOrCreate(TestEmployee1Login, true);
                          var principal3 = principalBll.GetByNameOrCreate(TestEmployee3Login, true);

                          var entityType = authContext.Logics.EntityType.GetByName(nameof(BusinessUnit));

                          Expression<Func<PermissionFilterEntity, bool>> entitySearchFilter =
                                  entity =>
                                          entity.EntityType == entityType
                                          && entity.EntityId == DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID;

                          var filterEntity = authContext.Logics.PermissionFilterEntity.GetObjectBy(entitySearchFilter) ?? new PermissionFilterEntity
                                                 {
                                                         EntityType = entityType,
                                                         EntityId = DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID
                                                 }.Self(bu => authContext.Logics.PermissionFilterEntity.Save(bu));

                          var permission = new Permission(principal1);
                          permission.Role = authContext.Logics.BusinessRole.GetOrCreateAdminRole();

                          var role1 = SampleSystemSecurityRole.TestRole1;

                          var permission1 = new Permission(principal2) { Role = context.Logics.bu role1 };

                          var role2 = new BusinessRole { Name = TestEmployee3Login };

                          var employeePositionView = authContext.Logics.Operation.GetByName(SampleSystemSecurityOperation.EmployeePositionView.ToString());

                          var link2 = new BusinessRoleOperationLink(role2) { Operation = employeePositionView };

                          authContext.Logics.BusinessRole.Save(role2);

                          var permission2 = new Permission(principal3) { Role = role2 };

                          new PermissionFilterItem(permission) { Entity = filterEntity };

                          principalBll.Save(principal1);
                          principalBll.Save(principal2);
                          principalBll.Save(principal3);
                      });
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
        var actual = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$filter=CoreBusinessUnit ne null"))
                               .Items.Select(dto => dto.Login);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void EmployeeProjectionSecurityTestNoAccess()
    {
        // Arrange
        var controller = this.GetControllerEvaluator<EmployeeQueryController>(TestEmployee1Login);

        // Act
        var result = controller.Evaluate(c => c.GetTestEmployeesByODataQueryString("$filter=CoreBusinessUnit ne null")).Items;

        // Assert
        var positions = result.Select(dto => dto.PositionName);
        var logins = result.Select(dto => dto.Login);
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
