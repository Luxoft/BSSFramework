using Framework.DomainDriven;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class EventDALListenerTests : TestBase
{
    [TestMethod]
    public void Employee_SendCustomEventOperation_ExceptionNotThrow()
    {
        //Arrange

        // Act
        var action = () => this.EvaluateAsync(
            DBSessionMode.Write,
            ctx => ctx.OperationSender.Send(
                ctx.Logics.Employee.GetCurrent(),
                SampleSystemEventOperation.CustomAction,
                CancellationToken.None));

        // Assert
        action.Should().NotThrowAsync();
    }
}
