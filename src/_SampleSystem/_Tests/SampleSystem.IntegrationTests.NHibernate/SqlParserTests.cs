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
        var ex = Record.Exception(() => this.EvaluateWrite(context => { context.Logics.SqlParserTestObj.Save(testObject); }));

        // Assert
        var requiredConstraintException = Assert.IsType<RequiredConstraintDALException>(ex);
        Assert.Contains("The field \'notNullColumn\' of type SqlParserTestObj must be initialized", requiredConstraintException.Message);
    }

    [Fact]
    public void SaveNotUniqueProperty_UniqueViolationConstraintDALException()
    {
        // Arrange
        var testObject1 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };
        var testObject2 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };

        // Act
        var ex = Record.Exception(() => this.EvaluateWrite(
            context =>
            {
                context.Logics.SqlParserTestObj.Save(testObject1);
                context.Logics.SqlParserTestObj.Save(testObject2);
            }));

        // Assert
        var uniqueViolationException = Assert.IsType<UniqueViolationConstraintDALException>(ex);
        Assert.Contains("SqlParserTestObj with same:\'UniqueColumn\' already exists", uniqueViolationException.Message);
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
        var ex = Record.Exception(() => this.EvaluateWrite(
            context =>
            {
                context.Logics.SqlParserTestObj.Remove(testObject);
            }));

        // Assert
        var removeLinkedObjectsException = Assert.IsType<RemoveLinkedObjectsDALException>(ex);
        Assert.Contains($"{nameof(SqlParserTestObj)} cannot be removed because it is used in {nameof(SqlParserTestObjContainer)}", removeLinkedObjectsException.Message);
    }

    [Fact]
    public void RemoveHRDepartment_HasEmployeeWithHRDepartment_CorrectExceptionMessage()
    {
        // Arrange
        var employeeController = this.MainWebApi.Employee;
        var hRDepartmentController = this.GetControllerEvaluator<HRDepartmentController>();

        var buTypeId = this.DataManager.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

        var luxoftBuId = this.DataManager.SaveBusinessUnit(
                                                          id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                                                          name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                                                          type: buTypeId);

        var costBuId = this.DataManager.SaveBusinessUnit(
                                                        id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                                                        name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                                                        type: buTypeId,
                                                        parent: luxoftBuId);

        var location = this.DataManager.SaveLocation(name: "testLocation");

        var employeeIdentity = this.DataManager.SaveEmployee(login: "value", coreBusinessUnit: costBuId, location: location);

        var fullEmployee = employeeController.Evaluate(c => c.GetFullEmployee(employeeIdentity));

        // Act
        var ex = Record.Exception(() => hRDepartmentController.Evaluate(c => c.RemoveHRDepartment(fullEmployee.HRDepartment.Identity)));

        // Assert
        var removeLinkedObjectsException = Assert.IsType<RemoveLinkedObjectsDALException>(ex);
        Assert.Equal($"{nameof(HRDepartment)} cannot be removed because it is used in {nameof(Employee)}", removeLinkedObjectsException.Message);
    }
}
