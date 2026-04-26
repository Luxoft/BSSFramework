using Framework.Application;
using Framework.AutomationCore.Utils;
using Framework.Database;

using Anch.SecuritySystem.UserSource;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class CurrentUserSourceTests : TestBase
{
    [Fact]
    public void TryGetCurrentUserWithEmployee_CurrentUserResolved()
    {
        // Arrange
        var randomName = TextRandomizer.RandomString(10);
        var employeeId = this.DataHelper.SaveEmployee(login: randomName).Id;

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            randomName,
            ctx =>
            {
                var emp = ctx.CurrentEmployeeSource.CurrentUser;

                return emp.Id;
            });

        // Assert
        Assert.Equal(result, employeeId);
    }

    [Fact]
    public void TryGetCurrentUserWithoutEmployee_ExceptionRaised()
    {
        // Arrange
        var randomName = TextRandomizer.RandomString(10);

        // Act
        var action = () =>
        {
            this.Evaluate(
                DBSessionMode.Read,
                randomName,
                ctx => ctx.CurrentEmployeeSource.CurrentUser.Id);
        };

        // Assert
        var exception = Assert.Throws<UserSourceException>(action);
        Assert.Equal($"{nameof(Employee)} \"{randomName}\" not found", exception.Message);
    }
}
