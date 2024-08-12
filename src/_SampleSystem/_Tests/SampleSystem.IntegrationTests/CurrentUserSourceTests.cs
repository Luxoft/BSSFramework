using Automation.Utils;

using FluentAssertions;

using Framework.Authorization.SecuritySystem.UserSource;
using Framework.DomainDriven;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class CurrentUserSourceTests : TestBase
{
    [TestMethod]
    public void TryGetCurrentUserWithEmployee_CurrentUserResolved()
    {
        // Arrange
        var randomName = TextRandomizer.RandomString(10);
        var employeeId = this.DataHelper.SaveEmployee(login: randomName).Id;

        // Act
        var result = this.Evaluate(DBSessionMode.Read, randomName, ctx => ctx.ServiceProvider.GetRequiredService<ICurrentUserSource>().CurrentUserId);

        // Assert
        employeeId.Should().Be(result);
    }

    [TestMethod]
    public void TryGetCurrentUserWithoutEmployee_ExceptionRaised()
    {
        // Arrange
        var randomName = TextRandomizer.RandomString(10);

        // Act
        var action = () => this.Evaluate(DBSessionMode.Read, randomName, ctx => ctx.ServiceProvider.GetRequiredService<ICurrentUserSource>().CurrentUserId);

        // Assert
        action.Should().Throw<Exception>()
              .And.Message.Should().Contain(
                  $"{nameof(Employee)} with {nameof(Employee.Login)} ({randomName}) not found");
    }
}
