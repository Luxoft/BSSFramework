using Framework.Application;
using Framework.AutomationCore.Utils;
using Framework.Database;

using SecuritySystem.UserSource;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests.__Support.TestData;

using Xunit;

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
        employeeId.Should().Be(result);
    }

    [Fact]
    public void TryGetCurrentUserWithoutEmployee_ExceptionRaised()
    {
        // Arrange
        var randomName = TextRandomizer.RandomString(10);

        // Act
        var action = () => this.Evaluate(
                         DBSessionMode.Read,
                         randomName,
                         ctx => ctx.CurrentEmployeeSource.CurrentUser.Id);

        // Assert
        action.Should().Throw<UserSourceException>().And.Message.Should().Be($"{nameof(Employee)} \"{randomName}\" not found");
    }
}
