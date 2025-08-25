using Automation.ServiceEnvironment;

using Framework.DomainDriven;
using SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class CurrentUserManyRelativePathTests : TestBase
{
    private Guid[] testEmployeeIdents;

    private Guid testObj;

    [TestInitialize]
    public void Setup()
    {
        this.testEmployeeIdents =

            Enumerable.Range(0, 3).Select(
                          _ => { return this.DataHelper.SaveEmployee().Id; })
                      .ToArray();

        this.testObj = this.EvaluateWrite(
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestRelativeEmployeeParentObject>();

                var parentObj = new TestRelativeEmployeeParentObject();

                foreach (var testEmployeeIdent in this.testEmployeeIdents)
                {
                    var testEmployee = ctx.Logics.Employee.GetById(testEmployeeIdent, true);

                    new TestRelativeEmployeeChildObject(parentObj) { Employee = testEmployee };
                }

                bll.Save(parentObj);

                return parentObj.Id;
            });
    }

    [TestMethod]
    public void TestManyRelativeEmployeeObject_FilterByEmployee_ObjectFound()
    {
        // Arrange

        var currentUserId = this.EvaluateRead(ctx => ctx.Authorization.CurrentUser.Id);

        Guid[] allTestIdents = [currentUserId, .. this.testEmployeeIdents];

        bool[] expectedResult = [false, .. this.testEmployeeIdents.Select(_ => true)];

        // Act
        var results =

            allTestIdents.Select(
                             employeeId =>
                                 this.Evaluate(
                                     DBSessionMode.Read,
                                     employeeId,
                                     ctx => ctx.Logics
                                               .Default
                                               .Create<TestRelativeEmployeeParentObject>(DomainSecurityRule.CurrentUser)
                                               .GetSecureQueryable()
                                               .Any(obj => obj.Id == this.testObj)))
                         .ToArray();

        // Assert
        results.Should().BeEquivalentTo(expectedResult);
    }
}
