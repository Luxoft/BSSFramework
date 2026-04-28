using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Database.DALExceptions;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.HRDepartment;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class SqlParserTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void SaveNullIntoNotNullProperty_RequiredConstraintDALException()
    {
        // Arrange
        var testObject = new SqlParserTestObj();

        // Act
        var action = () => this.EvaluateWrite(context => { context.Logics.SqlParserTestObj.Save(testObject); });

        // Assert
        Assert.Contains("The field \'notNullColumn\' of type SqlParserTestObj must be initialized", Assert.Throws<RequiredConstraintDALException>(action).Message);
    }

    [Fact]
    public void SaveNotUniqueProperty_UniqueViolationConstraintDALException()
    {
        // Arrange
        var testObject1 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };
        var testObject2 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };

        // Act
        var action = () => this.EvaluateWrite(
                                                 context =>
                                                 {
                                                     context.Logics.SqlParserTestObj.Save(testObject1);
                                                     context.Logics.SqlParserTestObj.Save(testObject2);
                                                 });

        // Assert
        Assert.Contains("SqlParserTestObj with same:\'UniqueColumn\' already exists", Assert.Throws<UniqueViolationConstraintDALException>(action).Message);
    }

    [Fact]
    public void RemoveLinkedObject_RemoveLinkedObjectsDALException()
    {
        // Arrange
        var testObject = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };
        var testObjectContainer = new SqlParserTestObjContainer { IncludedObject = testObject };

        this.EvaluateWrite(
                           context =>
                           {
                               context.Logics.SqlParserTestObj.Save(testObject);
                               context.Logics.SqlParserTestObjContainer.Save(testObjectContainer);
                           });

        // Act
        var action = () => this.EvaluateWrite(
                                                 context =>
                                                 {
                                                     context.Logics.SqlParserTestObj.Remove(testObject);
                                                 });

        // Assert
        Assert.Contains($"{nameof(SqlParserTestObj)} cannot be removed because it is used in {nameof(SqlParserTestObjContainer)}", Assert.Throws<RemoveLinkedObjectsDALException>(action).Message);
    }

    [Fact]
    public void RemoveHRDepartment_HasEmployeeWithHRDepartment_CorrectExceptionMessage()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var hRDepartmentController = this.GetControllerEvaluator<HRDepartmentController>();

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

        var location = this.DataHelper.SaveLocation(name: "testLocation");

        var employeeIdentity = this.DataHelper.SaveEmployee(login: "value", coreBusinessUnit: costBuId, location: location);

        var fullEmployee = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));

        // Act
        var action = () => hRDepartmentController.Evaluate(c => c.RemoveHRDepartment(fullEmployee.HRDepartment.Identity));

        // Assert
        Assert.Equal($"{nameof(HRDepartment)} cannot be removed because it is used in {nameof(Employee)}", Assert.Throws<RemoveLinkedObjectsDALException>(action).Message);
    }
}
